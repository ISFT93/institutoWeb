using instituto93.Domain.Models;

namespace instituto93.Application.Interfaces
{
    public interface IPersonalService
    {
        Task<List<Personal>> GetPersonalAsync(CancellationToken cancellationToken = default);
        Task<Personal?> GetPersonalByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CreatePersonalAsync(Personal parametros, CancellationToken cancellationToken = default);
        Task<bool> UpdatePersonalAsync(Personal parametros, CancellationToken cancellationToken = default);
        Task<bool> DeletePersonalAsync(int id, CancellationToken cancellationToken = default);
    }
}
