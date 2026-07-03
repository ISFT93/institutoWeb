using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using instituto93.Domain.Models;

namespace instituto93.Data.Repositories
{
    //lopez melany
    public interface IAlumnoRepository
    {
        Task<IEnumerable<AlumnoModelo>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<AlumnoModelo?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CreateAsync(AlumnoModelo alumno, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(AlumnoModelo alumno, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}