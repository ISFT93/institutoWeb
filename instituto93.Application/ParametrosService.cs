using instituto93.Application.Interfaces;
using instituto93.Data.Repositories.Interfaces;
using instituto93.Domain.Models;

namespace instituto93.Application
{
    public class ParametrosService : IParametrosService
    {
        private readonly IParametrosRepository _repo;

        public ParametrosService(IParametrosRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<List<Parametros>> GetParametrosAsync(CancellationToken cancellationToken = default)
        {
            var result = await _repo.GetAllAsync(cancellationToken);
            return result.ToList();
        }

        public Task<Parametros?> GetParametroByIdAsync(int id, CancellationToken cancellationToken = default)
            => _repo.GetByIdAsync(id, cancellationToken);

        public Task<Parametros?> GetParametroByNameAsync(string nombre, CancellationToken cancellationToken = default)
            => _repo.GetByNameAsync(nombre, cancellationToken);

        public Task<int> CreateParametroAsync(Parametros parametros, CancellationToken cancellationToken = default)
            => _repo.CreateAsync(parametros, cancellationToken);

        public Task<bool> UpdateParametroAsync(Parametros parametros, CancellationToken cancellationToken = default)
            => _repo.UpdateAsync(parametros, cancellationToken);

        public Task<bool> UpdateParametrosRangeAsync(IEnumerable<Parametros> parametros, CancellationToken cancellationToken = default)
            => _repo.UpdateRangeAsync(parametros, cancellationToken);

        public Task<bool> DeleteParametroAsync(int id, CancellationToken cancellationToken = default)
            => _repo.DeleteAsync(id, cancellationToken);
    }
}
