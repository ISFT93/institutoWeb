using instituto93.Web.Mocks;
using instituto93.Web.Models;

namespace instituto93.Web.Services;

/// <summary>
/// Oferta de materias y mesas finales para alumnos. El backend todavía no tiene endpoints para esto,
/// por lo que fuera de Development devuelve listas vacías.
/// </summary>
public sealed class OfertaAcademicaService(MockDataStore? mocks = null)
{
    public Task<List<MateriaInscripcionModel>> GetMateriasAsync() =>
        Task.FromResult(mocks?.MateriasInscripcion() ?? []);

    public Task<List<MateriaInscripcionModel>> GetInscripcionesAsync(int alumnoId) =>
        Task.FromResult(mocks?.MateriasInscriptas(alumnoId) ?? []);

    public Task<bool> InscribirAsync(int alumnoId, IEnumerable<int> materiaIds)
    {
        if (mocks is null)
            return Task.FromResult(false);

        mocks.InscribirMaterias(alumnoId, materiaIds);
        return Task.FromResult(true);
    }

    public Task<List<MesaFinalInscripcionModel>> GetMesasAsync() =>
        Task.FromResult(mocks?.MesasAlumno() ?? []);
}
