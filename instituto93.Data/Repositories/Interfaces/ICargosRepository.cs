using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using instituto93.Domain.Models;

namespace instituto93.Data.Repositories
{
    public interface ICargosRepository
    {
        Task<IEnumerable<Cargo>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Cargo?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CreateAsync(Cargo cargo, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(Cargo cargo, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}