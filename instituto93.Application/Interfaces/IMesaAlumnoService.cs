namespace instituto93.Application.Interfaces
{
    public interface IMesaAlumnoService
    {
        Task<int> InscribirAsync(int alumnoId, List<int> mesaFinalIds, CancellationToken cancellationToken = default);
    }
}
