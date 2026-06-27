using instituto93.Domain.Models;

namespace instituto93.Application.Interfaces
{
    public interface IPersonalService
    {
        Task<List<PersonalModelo>> GetPersonalAsync(CancellationToken cancellationToken = default);
        Task<PersonalModelo?> GetPersonalByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CreatePersonalAsync(PersonalModelo parametros, CancellationToken cancellationToken = default);
        Task<bool> UpdatePersonalAsync(PersonalModelo parametros, CancellationToken cancellationToken = default);
        Task<bool> DeletePersonalAsync(int id, CancellationToken cancellationToken = default);
    }
}
