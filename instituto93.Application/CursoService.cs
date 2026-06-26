using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using instituto93.Data.Repositories;
using instituto93.Domain.Models;


namespace instituto93.Application.Interfaces
{
    public class CursoService : ICursoService
    {
         private readonly ICursoRepository _repo;
         public CursoService(ICursoRepository repo)
         {
             _repo = repo ?? throw new System.ArgumentNullException(nameof(repo));
         }
         public async Task<List<CursoModelo>> GetCursos(CancellationToken cancellationToken = default)
         {
             var cursos = await _repo.GetAllAsync(cancellationToken);
             return cursos.ToList();
        }
    }
}
