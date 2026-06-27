using instituto93.Application.Interfaces;
using instituto93.Data.Repositories.Interfaces;
using instituto93.Domain.Models;

namespace instituto93.Application
{
    public class PersonalService : IPersonalService
    {
        private readonly IPersonalRepository _repo;

        public PersonalService(IPersonalRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<List<PersonalModelo>> GetPersonalAsync(CancellationToken cancellationToken = default)
        {
            var result = await _repo.GetAllAsync(cancellationToken);
            return result.ToList();
        }

        public Task<PersonalModelo?> GetPersonalByIdAsync(int id, CancellationToken cancellationToken = default)
            => _repo.GetByIdAsync(id, cancellationToken);


        public Task<int> CreatePersonalAsync(PersonalModelo personal, CancellationToken cancellationToken = default)
            => _repo.CreateAsync(personal, cancellationToken);

        public Task<bool> UpdatePersonalAsync(PersonalModelo personal, CancellationToken cancellationToken = default)
            => _repo.UpdateAsync(personal, cancellationToken);


        public Task<bool> DeletePersonalAsync(int id, CancellationToken cancellationToken = default)
            => _repo.DeleteAsync(id, cancellationToken);
    }
}
