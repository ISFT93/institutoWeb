using instituto93.Application.Interfaces;
using instituto93.Data.Repositories.Interfaces;
using instituto93.Domain.Models;

namespace instituto93.Application
{
    public class PaisService : IPaisService
    {
        private readonly IPaisRepository _repo;

        public PaisService(IPaisRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<List<Pais>> GetPaisAsync(CancellationToken cancellationToken = default)
        {
            var result = await _repo.GetAllAsync(cancellationToken);
            return result.ToList();
        }

        public Task<Pais?> GetPaisByIdAsync(int id, CancellationToken cancellationToken = default)
            => _repo.GetByIdAsync(id, cancellationToken);


        public Task<int> CreatePaisAsync(Pais pais, CancellationToken cancellationToken = default)
            => _repo.CreateAsync(pais, cancellationToken);

        public Task<bool> UpdatePaisAsync(Pais pais, CancellationToken cancellationToken = default)
            => _repo.UpdateAsync(pais, cancellationToken);


        public Task<bool> DeletePaisAsync(int id, CancellationToken cancellationToken = default)
            => _repo.DeleteAsync(id, cancellationToken);
    }
}
