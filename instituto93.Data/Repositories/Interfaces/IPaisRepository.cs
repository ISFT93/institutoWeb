using instituto93.Domain.Models;

namespace instituto93.Data.Repositories.Interfaces
{
    public interface IPaisRepository
    {
        Task<IEnumerable<Pais>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Pais?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CreateAsync(Pais pais, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(Pais pais, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}