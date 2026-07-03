using instituto93.Data.Repositories;
using instituto93.Domain.Models;

namespace instituto93.Application
{
    public class CargosService : ICargosService
    {
        private readonly ICargosRepository _repo;
        public CargosService(ICargosRepository repo)
        {
            _repo = repo ?? throw new System.ArgumentNullException(nameof(repo));
        }
        public async Task<List<Cargo>> GetCargos(CancellationToken cancellationToken = default)
        {
            var cargos = await _repo.GetAllAsync(cancellationToken);
            return cargos.ToList();
        }
        public async Task<Cargo> GetByIdCargos(int id, CancellationToken cancellationToken = default)
        {
            var cargo = await _repo.GetByIdAsync(id, cancellationToken);
            return cargo;
        }
        public async Task<int> CreateCargo(Cargo cargo, CancellationToken cancellationToken = default)
        {
            if (cargo == null)
                throw new ArgumentNullException(nameof(cargo));
            return await _repo.CreateAsync(cargo, cancellationToken);
        }
        public async Task<bool> UpdateCargo(Cargo cargo, CancellationToken cancellationToken = default)
        {
            if (cargo == null)
                throw new ArgumentNullException(nameof(cargo));
            return await _repo.UpdateAsync(cargo, cancellationToken);
        }
        public async Task<bool> DeleteCargo(int id, CancellationToken cancellationToken = default)
        {
            return await _repo.DeleteAsync(id, cancellationToken);
        }
    }
}