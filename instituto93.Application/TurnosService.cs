using instituto93.Application.Interfaces;
using instituto93.Data.Repositories.Interfaces;
using instituto93.Domain.Models;

namespace instituto93.Application
{
    public class TurnosService : ITurnosService
    {
        private readonly ITurnosRepository _repo;
        public TurnosService(ITurnosRepository repo)
        {
            _repo = repo ?? throw new System.ArgumentNullException(nameof(repo));
        }

        public async Task<List<TurnosLookup>> GetLookupAsync(CancellationToken cancellationToken = default)
        {
            var turnos = await _repo.GetLookupAsync(cancellationToken);
            return turnos.ToList();
        }
    }
}
