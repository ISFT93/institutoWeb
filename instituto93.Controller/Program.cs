using instituto93.Application;
using instituto93.Application.Interfaces;
using instituto93.Data;
using instituto93.Data.Repositories;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using WebApplication1.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Configuraci�n simple: puedes mover esto a appsettings.json / User Secrets
builder.Configuration["Jwt:Secret"] ??= "4d6d6a6d6a6d6a6d6a6d6a6d6a6d6a6d6a6d6a6d6a6d6a6d6a6d6a6d6a6d6a6d";

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddTransient<instituto93.Data.Conexion>();
builder.Services.AddScoped<instituto93.Data.Repositories.ILocalidadRepository, instituto93.Data.Repositories.LocalidadRepository>();
builder.Services.AddScoped<ILocalidadService, LocalidadService>();
builder.Services.AddScoped<instituto93.Data.Repositories.Interfaces.IInscripcionMateriaRepository, instituto93.Data.Repositories.InscripcionMateriaRepository>();
builder.Services.AddScoped<IInscripcionMateriaService, InscripcionMateriaService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Usar: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement{
            {
                new OpenApiSecurityScheme{ Reference = new OpenApiReference{ Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
                new string[] {}
            }
        });
});

var app = builder.Build();

// Pipeline
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// Insertar middleware JWT antes de UseAuthorization
app.UseMiddleware<JwtMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();

