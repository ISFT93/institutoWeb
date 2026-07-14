using instituto93.Application.Interfaces;
using instituto93.Data.Repositories.Interfaces;
using instituto93.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instituto93.Application
{
    public class MateriasModeloService:IMateriasModeloService
    {
        private readonly IMateriasModeloRepository _repo;
        public MateriasModeloService(IMateriasModeloRepository repo)
        {
            _repo = repo ?? throw new System.ArgumentNullException(nameof(repo));
        }
        public async Task<List<MateriasModelo>> GetMateriasModelo(CancellationToken cancellationToken = default)
        {
            var materiasModelo = await _repo.GetAllAsync(cancellationToken);
            return materiasModelo.ToList();
        }
    }
}
