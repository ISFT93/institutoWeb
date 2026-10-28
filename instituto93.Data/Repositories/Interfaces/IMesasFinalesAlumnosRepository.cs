namespace instituto93.Data.Repositories.Interfaces
{
    public interface IMesasFinalesAlumnosRepository
    {
        Task<List<int>> GetInscritosMesaIdsAsync(int alumnoId, IEnumerable<int> mesaFinalIds, CancellationToken cancellationToken = default);
        Task<int> InscribirAsync(int alumnoId, IEnumerable<int> mesaFinalIds, CancellationToken cancellationToken = default);
    }
}
