using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using instituto93.Domain.Models;

namespace instituto93.Application
{
    public interface ICicloLectivoService
    {
        Task<List<CicloLectivoModelo>> GetCiclosLectivosAsync(CancellationToken cancellationToken = default);
        Task<CicloLectivoModelo?> GetByAnioAsync(int anioLectivo, CancellationToken cancellationToken = default);
        Task<int> CreateAsync(CicloLectivoModelo cicloLectivo, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(CicloLectivoModelo cicloLectivo, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int anioLectivo, CancellationToken cancellationToken = default);
    }
}