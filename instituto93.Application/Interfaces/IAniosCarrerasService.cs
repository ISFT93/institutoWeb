using instituto93.Domain.Models;

namespace instituto93.Application.Interfaces
{
    public interface IAniosCarrerasService
    {
        Task<List<AnioCarreraLookup>> GetLookupAsync(int carreraId, CancellationToken cancellationToken = default);
    }
}
