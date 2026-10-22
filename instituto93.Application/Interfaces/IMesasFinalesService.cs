using instituto93.Domain.Models;

namespace instituto93.Application.Interfaces
{
    public interface IMesasFinalesService
    {
        Task<List<MesaFinalFila>> GetListAsync(MesasFinalesFiltro filtro, CancellationToken cancellationToken = default);
    }
}
