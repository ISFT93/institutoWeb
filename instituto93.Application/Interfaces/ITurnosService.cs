using instituto93.Domain.Models;

namespace instituto93.Application.Interfaces
{
    public interface ITurnosService
    {
        Task<List<TurnosLookup>> GetLookupAsync(CancellationToken cancellationToken = default);
    }
}
