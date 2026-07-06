using instituto93.Domain.Models;

namespace instituto93.Application.Interfaces
{
    public interface IPaisService
    {
        Task<List<Pais>> GetPaisAsync(CancellationToken cancellationToken = default);
        Task<Pais?> GetPaisByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CreatePaisAsync(Pais pais, CancellationToken cancellationToken = default);
        Task<bool> UpdatePaisAsync(Pais pais, CancellationToken cancellationToken = default);
        Task<bool> DeletePaisAsync(int id, CancellationToken cancellationToken = default);
    }
}
