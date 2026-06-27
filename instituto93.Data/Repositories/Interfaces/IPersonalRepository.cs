using instituto93.Domain.Models;

namespace instituto93.Data.Repositories.Interfaces
{
    public interface IPersonalRepository
    {
        Task<IEnumerable<PersonalModelo>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<PersonalModelo?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CreateAsync(PersonalModelo personal, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(PersonalModelo personal, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
