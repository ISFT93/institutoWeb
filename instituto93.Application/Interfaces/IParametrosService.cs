using instituto93.Domain.Models;

namespace instituto93.Application.Interfaces
{
    public interface IParametrosService
    {
        Task<List<Parametros>> GetParametrosAsync(CancellationToken cancellationToken = default);
        Task<Parametros?> GetParametroByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Parametros?> GetParametroByNameAsync(string nombre, CancellationToken cancellationToken = default);
        Task<int> CreateParametroAsync(Parametros parametros, CancellationToken cancellationToken = default);
        Task<bool> UpdateParametroAsync(Parametros parametros, CancellationToken cancellationToken = default);
        Task<bool> UpdateParametrosRangeAsync(IEnumerable<Parametros> parametros, CancellationToken cancellationToken = default);
        Task<bool> DeleteParametroAsync(int id, CancellationToken cancellationToken = default);
    }
}
