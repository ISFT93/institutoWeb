using instituto93.Domain.Models;

namespace instituto93.Data.Repositories.Interfaces
{
    public interface IParametrosRepository
    {
        Task<IEnumerable<Parametros>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Parametros?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Parametros?> GetByNameAsync(string nombre, CancellationToken cancellationToken = default);
        Task<int> CreateAsync(Parametros parametros, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(Parametros parametros, CancellationToken cancellationToken = default);
        Task<bool> UpdateRangeAsync(IEnumerable<Parametros> parametros, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
