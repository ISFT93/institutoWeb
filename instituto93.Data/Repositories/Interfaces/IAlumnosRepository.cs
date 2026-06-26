using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using instituto93.Domain.Models;

namespace instituto93.Data.Repositories
{
    //lopez melany
    public interface IAlumnosRepository
    {
        Task<IEnumerable<AlumnosModelo>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<AlumnosModelo?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CreateAsync(AlumnosModelo alumno, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(AlumnosModelo alumno, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}