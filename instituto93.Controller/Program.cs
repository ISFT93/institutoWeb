using instituto93.Application;
using instituto93.Application.Interfaces;
using instituto93.Data;
using instituto93.Data.Repositories;
using instituto93.Data.Repositories.Interfaces;
using instituto93.Controller.Seeds;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

Env.NoClobber().TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

var jwtSettings = CreateJwtSettings(builder.Configuration, builder.Environment);
builder.Services.AddSingleton(jwtSettings);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(jwtSettings.SigningKey),
            ValidAlgorithms = [SecurityAlgorithms.HmacSha256],
            NameClaimType = JwtRegisteredClaimNames.Name,
            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });
builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddTransient<instituto93.Data.Conexion>();
builder.Services.AddScoped<instituto93.Data.Repositories.ILocalidadRepository, instituto93.Data.Repositories.LocalidadRepository>();
builder.Services.AddScoped<ILocalidadService, LocalidadService>();
builder.Services.AddScoped<instituto93.Data.Repositories.ICargosRepository, instituto93.Data.Repositories.CargosRepository>();
builder.Services.AddScoped<ICargosService, CargosService>();
builder.Services.AddScoped<IPaisRepository, PaisRepository>();
builder.Services.AddScoped<IPaisService, PaisService>();
builder.Services.AddScoped<IInscripcionMateriaRepository, InscripcionMateriaRepository>();
builder.Services.AddScoped<IInscripcionMateriaService, InscripcionMateriaService>();
builder.Services.AddScoped<IPersonalRepository, PersonalRepository>();
builder.Services.AddScoped<IPersonalService, PersonalService>();
builder.Services.AddScoped<IParametroRepository, ParametroRepository>();
builder.Services.AddScoped<IParametroService, ParametroService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IAlumnoRepository, AlumnoRepository>();
builder.Services.AddScoped<IAlumnoService, AlumnoService>();
builder.Services.AddScoped<IProfesorRepository, ProfesorRepository>();
builder.Services.AddScoped<IAlumnoAccesoService, AlumnoAccesoService>();
builder.Services.AddScoped<ICarrerasRepository, CarrerasRepository>();
builder.Services.AddScoped<ICarrerasService, CarrerasService>();
builder.Services.AddScoped<IAlumnosCarrerasRepository, AlumnosCarrerasRepository>();
builder.Services.AddScoped<IAlumnosCarrerasService, AlumnosCarrerasService>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IAuthTokenService, AuthTokenService>();
builder.Services.AddScoped<DevelopmentUserSeed>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Access token devuelto por api/Auth/login o api/Auth/refresh.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement{
            {
                new OpenApiSecurityScheme{ Reference = new OpenApiReference{ Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
                new string[] {}
            }
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    await scope.ServiceProvider.GetRequiredService<DevelopmentUserSeed>().SeedAsync();
}

// Pipeline
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

static JwtSettings CreateJwtSettings(IConfiguration configuration, IHostEnvironment environment)
{
    var secret = configuration["Jwt:Secret"];
    if (string.IsNullOrWhiteSpace(secret))
    {
        if (!environment.IsDevelopment())
            throw new InvalidOperationException("Falta la configuración 'Jwt:Secret' (variable de entorno Jwt__Secret).");

        // Solo para desarrollo local; en cualquier otro entorno el secreto es obligatorio.
        secret = "instituto93-desarrollo-local-no-usar-en-produccion";
    }

    var signingKey = Encoding.UTF8.GetBytes(secret);
    if (signingKey.Length < JwtSettings.MinimumSecretBytes)
        throw new InvalidOperationException($"'Jwt:Secret' debe tener al menos {JwtSettings.MinimumSecretBytes} bytes.");

    return new JwtSettings
    {
        Issuer = configuration["Jwt:Issuer"] ?? "instituto93.api",
        Audience = configuration["Jwt:Audience"] ?? "instituto93.web",
        SigningKey = signingKey,
        AccessTokenLifetime = TimeSpan.FromMinutes(configuration.GetValue("Jwt:AccessTokenMinutes", 15)),
        RefreshTokenLifetime = TimeSpan.FromDays(configuration.GetValue("Jwt:RefreshTokenDays", 14)),
        RefreshTokenAbsoluteLifetime = TimeSpan.FromDays(configuration.GetValue("Jwt:RefreshTokenAbsoluteDays", 30))
    };
}
