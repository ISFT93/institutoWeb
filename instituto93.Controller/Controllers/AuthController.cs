using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;
using instituto93.Application;
using instituto93.Application.Interfaces;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IAlumnoAccesoService _alumnoAccesoService;
        private readonly IAuthTokenService _authTokenService;

        public AuthController(
            IUsuarioService usuarioService,
            IAlumnoAccesoService alumnoAccesoService,
            IAuthTokenService authTokenService)
        {
            _usuarioService = usuarioService;
            _alumnoAccesoService = alumnoAccesoService;
            _authTokenService = authTokenService;
        }

        public class LoginRequest
        {
            public string Dni { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        public class RefreshTokenRequest
        {
            public string RefreshToken { get; set; } = string.Empty;
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

            var (estado, nombre) = await _alumnoAccesoService.ConsultarAccesoAsync(model.Dni, cancellationToken);
            return estado switch
            {
                EstadoAccesoDni.NoEncontrado => NotFound(new { message = "No encontramos un alumno con ese DNI. Revisá el número ingresado o comunicate con el instituto." }),
                EstadoAccesoDni.ConContrasena => Ok(new { estado = nameof(EstadoAccesoDni.ConContrasena), nombre }),
                _ => Ok(new { estado = nameof(EstadoAccesoDni.SinContrasena), nombre })
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
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> Login([FromBody] LoginRequest model, CancellationToken cancellationToken)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Dni) || string.IsNullOrWhiteSpace(model.Password))
                return BadRequest(new { message = "DNI y contraseña requeridos." });

            var usuario = await _usuarioService.AuthenticateAsync(model.Dni, model.Password, cancellationToken);
            if (usuario == null)
                return Unauthorized(new { message = "DNI o contraseña incorrectos." });

            var tokens = await _authTokenService.IssueAsync(usuario, cancellationToken);
            return Ok(TokenResponse.From(tokens));
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest model, CancellationToken cancellationToken)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.RefreshToken))
                return BadRequest(new { message = "Refresh token requerido." });

            var tokens = await _authTokenService.RefreshAsync(model.RefreshToken, cancellationToken);
            return tokens is null
                ? Unauthorized(new { message = "La sesión expiró. Iniciá sesión nuevamente." })
                : Ok(TokenResponse.From(tokens));
        }

        [HttpPost("logout")]
        [AllowAnonymous]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest model, CancellationToken cancellationToken)
        {
            if (model != null && !string.IsNullOrWhiteSpace(model.RefreshToken))
                await _authTokenService.RevokeAsync(model.RefreshToken, cancellationToken);

            return NoContent();
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            return Ok(new
            {
                usuarioId = int.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!),
                alumnoId = int.Parse(User.FindFirstValue(AuthTokenService.AlumnoIdClaim)!),
                nombre = User.FindFirstValue(JwtRegisteredClaimNames.Name) ?? string.Empty,
                email = User.FindFirstValue(JwtRegisteredClaimNames.Email)
            });
        }

        private sealed record TokenResponse(
            string AccessToken,
            string TokenType,
            int ExpiresIn,
            DateTime AccessTokenExpiresAt,
            string RefreshToken,
            DateTime RefreshTokenExpiresAt)
        {
            public static TokenResponse From(TokenPair tokens) => new(
                tokens.AccessToken,
                "Bearer",
                (int)Math.Max(0, (tokens.AccessTokenExpiresAt - DateTime.UtcNow).TotalSeconds),
                tokens.AccessTokenExpiresAt,
                tokens.RefreshToken,
                tokens.RefreshTokenExpiresAt);
        }
    }
}
