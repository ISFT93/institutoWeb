using instituto93.Application.Interfaces;
using instituto93.Data.Repositories.Interfaces;
using instituto93.Domain.Models;

namespace instituto93.Application
{
    public class ParametroService : IParametroService
    {
        private readonly IParametroRepository _repo;

        public ParametroService(IParametroRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<List<Parametro>> GetParametrosAsync(CancellationToken cancellationToken = default)
        {
            var result = await _repo.GetAllAsync(cancellationToken);
            return result.ToList();
        }

        public Task<Parametro?> GetParametroByIdAsync(int id, CancellationToken cancellationToken = default)
            => _repo.GetByIdAsync(id, cancellationToken);

        public Task<Parametro?> GetParametroByNameAsync(string nombre, CancellationToken cancellationToken = default)
            => _repo.GetByNameAsync(nombre, cancellationToken);

        public Task<int> CreateParametroAsync(Parametro parametros, CancellationToken cancellationToken = default)
            => _repo.CreateAsync(parametros, cancellationToken);

        public Task<bool> UpdateParametroAsync(Parametro parametros, CancellationToken cancellationToken = default)
            => _repo.UpdateAsync(parametros, cancellationToken);

        public Task<bool> UpdateParametrosRangeAsync(IEnumerable<Parametro> parametros, CancellationToken cancellationToken = default)
            => _repo.UpdateRangeAsync(parametros, cancellationToken);

        public Task<bool> DeleteParametroAsync(int id, CancellationToken cancellationToken = default)
            => _repo.DeleteAsync(id, cancellationToken);
    }
}
