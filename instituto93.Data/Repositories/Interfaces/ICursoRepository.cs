using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using instituto93.Domain.Models;

namespace instituto93.Data.Repositories
{
    public interface icursoRepository 
    {
        Task<IEnumerable<CursoModelo>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<CursoModelo?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CreateAsync(CursoModelo cursoModelo, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(CursoModelo cursoModelo, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}