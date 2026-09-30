using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using instituto93.Application.Interfaces;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IAlumnoAccesoService _alumnoAccesoService;
        private readonly IConfiguration _configuration;

        public AuthController(
            IUsuarioService usuarioService,
            IAlumnoAccesoService alumnoAccesoService,
            IConfiguration configuration)
        {
            _usuarioService = usuarioService;
            _alumnoAccesoService = alumnoAccesoService;
            _configuration = configuration;
        }

        public class LoginRequest
        {
            public string Dni { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        public class DniStatusRequest
        {
            public string Dni { get; set; } = string.Empty;
        }

        public class CreatePasswordRequest
        {
            public string Dni { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public string ConfirmPassword { get; set; } = string.Empty;
        }

        [HttpPost("dni-status")]
        [AllowAnonymous]
        public async Task<IActionResult> DniStatus([FromBody] DniStatusRequest model, CancellationToken cancellationToken)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Dni))
                return BadRequest(new { message = "Ingresá tu DNI." });

            var estado = await _alumnoAccesoService.ConsultarDniAsync(model.Dni, cancellationToken);
            return estado switch
            {
                EstadoAccesoDni.NoEncontrado => NotFound(new { message = "No encontramos un alumno con ese DNI. Revisá el número ingresado o comunicate con el instituto." }),
                EstadoAccesoDni.ConContrasena => Ok(new { estado = nameof(EstadoAccesoDni.ConContrasena) }),
                _ => Ok(new { estado = nameof(EstadoAccesoDni.SinContrasena) })
            };
        }

        [HttpPost("create-password")]
        [AllowAnonymous]
        public async Task<IActionResult> CreatePassword([FromBody] CreatePasswordRequest model, CancellationToken cancellationToken)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Dni))
                return BadRequest(new { message = "Ingresá tu DNI." });

            var resultado = await _alumnoAccesoService.CrearContrasenaAsync(
                model.Dni,
                model.Password,
                model.ConfirmPassword,
                cancellationToken);

            return resultado.Estado switch
            {
                CrearContrasenaEstado.Creada => Ok(new { message = "Contraseña creada correctamente." }),
                CrearContrasenaEstado.ContrasenaInvalida => BadRequest(new { message = resultado.Mensaje }),
                CrearContrasenaEstado.DniNoEncontrado => NotFound(new { message = "No encontramos un alumno con ese DNI." }),
                _ => Conflict(new { message = "Este DNI ya tiene una contraseña. Volvé al inicio para iniciar sesión." })
            };
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest model, CancellationToken cancellationToken)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Dni) || string.IsNullOrWhiteSpace(model.Password))
                return BadRequest(new { message = "DNI y contraseña requeridos." });

            var usuario = await _usuarioService.AuthenticateAsync(model.Dni, model.Password, cancellationToken);
            if (usuario == null)
                return Unauthorized(new { message = "DNI o contraseña incorrectos." });

            var alumno = usuario.Alumno!;

            var secret = _configuration["Jwt:Secret"] ?? "4d6d6a6d6a6d6a6d6a6d6a6d6a6d6a6d6a6d6a6d6a6d6a6d6a6d6a6d6a6d6a6d";
            var key = Encoding.UTF8.GetBytes(secret);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim("alumnoId", usuario.AlumnoId.ToString()),
                new Claim(ClaimTypes.Name, $"{alumno.Nombre} {alumno.Apellido}"),
                new Claim(ClaimTypes.Email, alumno.Email ?? string.Empty)
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
