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
using instituto93.Data.Repositories;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UsuarioRepository _repo;
        private readonly IConfiguration _configuration;

        public AuthController(UsuarioRepository repo, IConfiguration configuration)
        {
            _repo = repo;
            _configuration = configuration;
        }

        public class LoginRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class RegisterRequest
        {
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public DateTime FechaNacimiento { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Dni { get; set; }
            public string Telefono { get; set; }
            public string Direccion { get; set; }
            public int LocalidadId { get; set; }
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public IActionResult Register([FromBody] RegisterRequest model)
        {
            if (model == null
                || string.IsNullOrWhiteSpace(model.Email)
                || string.IsNullOrWhiteSpace(model.Password)
                || string.IsNullOrWhiteSpace(model.Nombre)
                || string.IsNullOrWhiteSpace(model.Apellido))
            {
                return BadRequest(new { message = "Datos de registro incompletos." });
            }

            // Verificar email único
            var exists = _repo.GetUsuarioByEmail(model.Email);
            if (exists != null)
                return Conflict(new { message = "Email ya en uso." });

            // Mapear a entidad Usuario. Dejar password en claro: AddUsuario la hashea.
            var usuario = new Usuario
            {
                Nombre = model.Nombre,
                Apellido = model.Apellido,
                FechaNacimiento = model.FechaNacimiento,
                Email = model.Email,
                Password = model.Password, // en claro: el repositorio lo transformará
                Dni = model.Dni,
                Telefono = model.Telefono,
                Direccion = model.Direccion,
                LocalidadId = model.LocalidadId
            };

            // Persistir (repositorio aplicará hashing)
            _repo.AddUsuario(usuario);

            // Construir DTO de respuesta sin contraseña
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
        public IActionResult Login([FromBody] LoginRequest model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
                return BadRequest(new { message = "Email y password requeridos." });

            if (!_repo.ValidateUser(model.Email, model.Password))
                return Unauthorized(new { message = "Credenciales inválidas." });

            var usuario = _repo.GetUsuarioByEmail(model.Email);
            if (usuario == null) return Unauthorized();

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