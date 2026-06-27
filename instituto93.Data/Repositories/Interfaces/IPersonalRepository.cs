using instituto93.Domain.Models;

namespace instituto93.Data.Repositories.Interfaces
{
    public interface IPersonalRepository
    {
        Task<IEnumerable<Personal>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Personal?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CreateAsync(Personal personal, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(Personal personal, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
