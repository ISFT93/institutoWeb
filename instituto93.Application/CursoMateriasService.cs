using instituto93.Data.Repositories.Interfaces;
using instituto93.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using instituto93.Application.Interfaces;

namespace instituto93.Application
{
    public class CursoMateriasService : ICursoMateriasService
    {
        private readonly ICursoMateriasRepository _repo;

        public CursoMateriasService(ICursoMateriasRepository repo)
        {
            _repo = repo ?? throw new System.ArgumentNullException(nameof(repo));
        }

        public async Task<List<CursoMaterias>> GetCursoMaterias(CancellationToken cancellationToken = default)
        {
            var cursoMaterias = await _repo.GetAllAsync(cancellationToken);
            return cursoMaterias.ToList();
        }
    }
}
