using instituto93.Domain.Models;

namespace instituto93.Data.Repositories.Interfaces
{
    public interface IMesasFinalesRepository
    {
        Task<IEnumerable<MesaFinalFila>> GetListAsync(MesasFinalesFiltro filtro, CancellationToken cancellationToken = default);
    }
}
