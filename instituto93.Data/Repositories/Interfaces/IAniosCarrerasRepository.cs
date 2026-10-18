using instituto93.Domain.Models;

namespace instituto93.Data.Repositories.Interfaces
{
    public interface IAniosCarrerasRepository
    {
        Task<IEnumerable<AnioCarreraLookup>> GetLookupAsync(int carreraId, CancellationToken cancellationToken = default);
    }
}
