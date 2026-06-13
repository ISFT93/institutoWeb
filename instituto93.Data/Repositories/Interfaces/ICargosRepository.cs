using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using instituto93.Domain.Models;

namespace instituto93.Data.Repositories
{
    public interface ICargosRepository
    {
        Task<IEnumerable<CargosModelo>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<CargosModelo?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CreateAsync(CargosModelo cargo, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(CargosModelo cargo, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}