using instituto93.Data.Repositories;
using instituto93.Domain.Models;

namespace instituto93.Application
{
    public class CargosService:ICargosService
    {
        private readonly ICargosRepository _repo;
        public CargosService(ICargosRepository  repo)
        {
            _repo = repo ?? throw new System.ArgumentNullException(nameof(repo));
        }
        public async Task<List<CargosModelo>> GetCargos(CancellationToken cancellationToken = default)
        {
            var cargos = await _repo.GetAllAsync(cancellationToken);
            return cargos.ToList();
        }

    }
}