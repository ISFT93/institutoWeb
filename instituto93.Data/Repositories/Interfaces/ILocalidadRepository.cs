using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using instituto93.Domain.Models;

namespace instituto93.Data.Repositories
{
    public interface ILocalidadRepository
    {
        Task<IEnumerable<Localidad>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Localidad?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CreateAsync(Localidad localidad, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(Localidad localidad, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}