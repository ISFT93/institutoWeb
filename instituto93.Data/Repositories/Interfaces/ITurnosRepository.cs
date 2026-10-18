using instituto93.Domain.Models;

namespace instituto93.Data.Repositories.Interfaces
{
    public interface ITurnosRepository
    {
        Task<IEnumerable<TurnosLookup>> GetLookupAsync(CancellationToken cancellationToken = default);
    }
}
