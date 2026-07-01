using instituto93.Domain.Models;

namespace instituto93.Data.Repositories.Interfaces
{
    public interface IParametroRepository
    {
        Task<IEnumerable<Parametro>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Parametro?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Parametro?> GetByNameAsync(string nombre, CancellationToken cancellationToken = default);
        Task<int> CreateAsync(Parametro parametros, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(Parametro parametros, CancellationToken cancellationToken = default);
        Task<bool> UpdateRangeAsync(IEnumerable<Parametro> parametros, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
