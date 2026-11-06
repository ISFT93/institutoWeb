using instituto93.Application;
using instituto93.Controller.Controllers;
using instituto93.Data.Repositories.Interfaces;
using instituto93.Domain.DTOs;
using instituto93.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace instituto93.Tests;

public class PreinscripcionPlazoTests
{
    private static readonly EstadoPreinscripcionDto Plazo = new()
    {
        Abierta = true,
        Desde = new DateTime(2026, 10, 1),
        Hasta = new DateTime(2026, 12, 15)
    };

    [Fact]
    public async Task Servicio_SinFilaVigente_DevuelveCerrada()
    {
        var service = new CicloLectivoService(new FakeCicloRepo(null));

        var estado = await service.GetEstadoPreinscripcionAsync();

        Assert.False(estado.Abierta);
        Assert.Null(estado.Desde);
        Assert.Null(estado.Hasta);
    }

    [Fact]
    public async Task Servicio_ConFilaVigente_DevuelveAbiertaConFechas()
    {
        var service = new CicloLectivoService(new FakeCicloRepo(Plazo));

        var estado = await service.GetEstadoPreinscripcionAsync();

        Assert.True(estado.Abierta);
        Assert.Equal(Plazo.Desde, estado.Desde);
        Assert.Equal(Plazo.Hasta, estado.Hasta);
    }

    [Fact]
    public async Task EndpointEstado_DevuelveElEstadoDelServicio()
    {
        var controller = new PreinscripcionController(new CicloLectivoService(new FakeCicloRepo(Plazo)));

        var result = await controller.GetEstado(CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.True(Assert.IsType<EstadoPreinscripcionDto>(ok.Value).Abierta);
    }

    [Fact]
    public async Task EndpointEstado_ErrorDeDatos_Devuelve500()
    {
        var controller = new PreinscripcionController(new CicloLectivoService(new FakeCicloRepo(null, throws: true)));

        var result = await controller.GetEstado(CancellationToken.None);

        Assert.Equal(500, Assert.IsType<ObjectResult>(result.Result).StatusCode);
    }

    [Fact]
    public async Task PostPreinscripcion_FueraDePlazo_Devuelve403YNoGuarda()
    {
        var alumnos = new FakeAlumnoService();
        var controller = new AlumnoController(alumnos, new CicloLectivoService(new FakeCicloRepo(null)));

        var result = await controller.CreatePreinscripcion(new PreinscripcionDto(), CancellationToken.None);

        Assert.Equal(403, Assert.IsType<ObjectResult>(result).StatusCode);
        Assert.False(alumnos.Guardo);
    }

    [Fact]
    public async Task PostPreinscripcion_DentroDePlazo_Guarda()
    {
        var alumnos = new FakeAlumnoService();
        var controller = new AlumnoController(alumnos, new CicloLectivoService(new FakeCicloRepo(Plazo)));

        var result = await controller.CreatePreinscripcion(new PreinscripcionDto(), CancellationToken.None);

        Assert.IsType<CreatedAtActionResult>(result);
        Assert.True(alumnos.Guardo);
    }

    private sealed class FakeCicloRepo(EstadoPreinscripcionDto? plazo, bool throws = false) : ICicloLectivoRepository
    {
        public Task<EstadoPreinscripcionDto?> GetPlazoPreinscripcionAbiertoAsync(CancellationToken cancellationToken = default) =>
            throws ? throw new InvalidOperationException("db caída") : Task.FromResult(plazo);

        public Task<IEnumerable<CicloLectivoModelo>> GetAllAsync(CancellationToken cancellationToken = default) => throw new NotImplementedException();
        public Task<CicloLectivoModelo?> GetByAnioAsync(int anioLectivo, CancellationToken cancellationToken = default) => throw new NotImplementedException();
        public Task<int> CreateAsync(CicloLectivoModelo cicloLectivo, CancellationToken cancellationToken = default) => throw new NotImplementedException();
        public Task<bool> UpdateAsync(CicloLectivoModelo cicloLectivo, CancellationToken cancellationToken = default) => throw new NotImplementedException();
        public Task<bool> DeleteAsync(int anioLectivo, CancellationToken cancellationToken = default) => throw new NotImplementedException();
    }

    private sealed class FakeAlumnoService : IAlumnoService
    {
        public bool Guardo { get; private set; }

        public Task<int> CreatePreinscripcionAsync(PreinscripcionDto preinscripcion, CancellationToken cancellationToken = default)
        {
            Guardo = true;
            return Task.FromResult(123);
        }

        public Task<List<AlumnoModelo>> GetAlumnosModelos(CancellationToken cancellationToken = default) => throw new NotImplementedException();
        public Task<AlumnoModelo?> GetAlumnoByIdAsync(int id, CancellationToken cancellationToken = default) => throw new NotImplementedException();
    }
}
