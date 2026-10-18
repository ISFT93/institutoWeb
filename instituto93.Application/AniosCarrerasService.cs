using instituto93.Application.Interfaces;
using instituto93.Data.Repositories.Interfaces;
using instituto93.Domain.Models;

namespace instituto93.Application
{
    public class AniosCarrerasService : IAniosCarrerasService
    {
        private readonly IAniosCarrerasRepository _repo;
        public AniosCarrerasService(IAniosCarrerasRepository repo)
        {
            _repo = repo ?? throw new System.ArgumentNullException(nameof(repo));
        }

        public async Task<List<AnioCarreraLookup>> GetLookupAsync(int carreraId, CancellationToken cancellationToken = default)
        {
            var anios = await _repo.GetLookupAsync(carreraId, cancellationToken);
            return anios.ToList();
        }
    }
}
