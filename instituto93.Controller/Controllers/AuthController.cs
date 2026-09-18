using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using instituto93.Domain.Models;
using instituto93.Domain.DTOs;
using Microsoft.Extensions.Configuration;
using instituto93.Application.Interfaces;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IConfiguration _configuration;

        public AuthController(IUsuarioService usuarioService, IConfiguration configuration)
        {
            _usuarioService = usuarioService;
            _configuration = configuration;
        }

        public class LoginRequest
        {
            public string EmailOrDni { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        // El registro se hace desde el lado de administración. Ahora dejamos el endpoint para pruebas, pero en producción se puede eliminar o proteger con autorización.
        public class RegisterRequest
        {
            public string Nombre { get; set; } = string.Empty;
            public string Apellido { get; set; } = string.Empty;
            public DateTime FechaNacimiento { get; set; }
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public string Dni { get; set; } = string.Empty;
            public string Telefono { get; set; } = string.Empty;
            public string Direccion { get; set; } = string.Empty;
            public int LocalidadId { get; set; }
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model, CancellationToken cancellationToken)
        {
            if (model == null
                || string.IsNullOrWhiteSpace(model.Email)
                || string.IsNullOrWhiteSpace(model.Password)
                || string.IsNullOrWhiteSpace(model.Nombre)
                || string.IsNullOrWhiteSpace(model.Apellido))
            {
                return BadRequest(new { message = "Datos de registro incompletos." });
            }

            // Verificar email �nico
            var exists = await _usuarioService.GetByEmailAsync(model.Email, cancellationToken);
            if (exists != null)
                return Conflict(new { message = "Email ya en uso." });

            // Mapear a entidad Usuario. Dejar password en claro: AddUsuario la hashea.
            var usuario = new Usuario
            {
                Nombre = model.Nombre,
                Apellido = model.Apellido,
                FechaNacimiento = model.FechaNacimiento,
                Email = model.Email,
                Password = model.Password, // en claro: el repositorio lo transformar�
                Dni = model.Dni,
                Telefono = model.Telefono,
                Direccion = model.Direccion,
                LocalidadId = model.LocalidadId,
                activo = true
            };

            // Persistir (repositorio aplicar� hashing)
            await _usuarioService.AddAsync(usuario, cancellationToken);

            // Construir DTO de respuesta sin contrase�a
            var dto = new UsuarioDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                FechaNacimiento = usuario.FechaNacimiento,
                Email = usuario.Email
            };

            // Devolver 201 con el recurso creado (location opcional)
            return CreatedAtAction(nameof(Register), new { id = dto.Id }, dto);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest model, CancellationToken cancellationToken)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.EmailOrDni) || string.IsNullOrWhiteSpace(model.Password))
                return BadRequest(new { message = "Email o DNI y contraseña requeridos." });

            var usuario = await _usuarioService.AuthenticateAsync(model.EmailOrDni, model.Password, cancellationToken);
            if (usuario == null)
                return Unauthorized(new { message = "Credenciales inválidas." });

            var secret = _configuration["Jwt:Secret"] ?? "4d6d6a6d6a6d6a6d6a6d6a6d6a6d6a6d6a6d6a6d6a6d6a6d6a6d6a6d6a6d6a6d";
            var key = Encoding.UTF8.GetBytes(secret);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{usuario.Nombre} {usuario.Apellido}"),
                new Claim(ClaimTypes.Email, usuario.Email ?? string.Empty)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return Ok(new
            {
                token = jwt,
                expires = token.ValidTo
            });
        }
    }
}
