using instituto93.Domain.Models;

namespace instituto93.Application.Interfaces
{
    public interface IParametroService
    {
        Task<List<Parametro>> GetParametrosAsync(CancellationToken cancellationToken = default);
        Task<Parametro?> GetParametroByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Parametro?> GetParametroByNameAsync(string nombre, CancellationToken cancellationToken = default);
        Task<int> CreateParametroAsync(Parametro parametros, CancellationToken cancellationToken = default);
        Task<bool> UpdateParametroAsync(Parametro parametros, CancellationToken cancellationToken = default);
        Task<bool> UpdateParametrosRangeAsync(IEnumerable<Parametro> parametros, CancellationToken cancellationToken = default);
        Task<bool> DeleteParametroAsync(int id, CancellationToken cancellationToken = default);
    }
}
