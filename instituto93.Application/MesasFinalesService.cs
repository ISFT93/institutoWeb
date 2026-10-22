using instituto93.Application.Interfaces;
using instituto93.Data.Repositories.Interfaces;
using instituto93.Domain.Models;

namespace instituto93.Application
{
    public class MesasFinalesService : IMesasFinalesService
    {
        private readonly IMesasFinalesRepository _repo;
        public MesasFinalesService(IMesasFinalesRepository repo)
        {
            _repo = repo ?? throw new System.ArgumentNullException(nameof(repo));
        }

        public async Task<List<MesaFinalFila>> GetListAsync(MesasFinalesFiltro filtro, CancellationToken cancellationToken = default)
        {
            var filas = await _repo.GetListAsync(filtro, cancellationToken);
            return filas.ToList();
        }
    }
}
