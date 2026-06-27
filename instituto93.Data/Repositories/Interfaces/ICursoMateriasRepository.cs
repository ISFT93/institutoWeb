using instituto93.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instituto93.Data.Repositories.Interfaces
{
    public interface ICursoMateriasRepository
    {
        Task<IEnumerable<CursoMaterias>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<CursoMaterias?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<int> CreateAsync(CursoMaterias cursoMaterias, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(CursoMaterias cursoMaterias, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default);

    }
}
