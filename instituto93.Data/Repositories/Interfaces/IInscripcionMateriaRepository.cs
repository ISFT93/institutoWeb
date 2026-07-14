using instituto93.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instituto93.Data.Repositories.Interfaces
{
    public interface IInscripcionMateriaRepository
    {
        Task<IEnumerable<InscripcionMateria>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<InscripcionMateria?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CreateAsync(int alumnoCarreraId, int cursadaId, int anioCicloLectivo, string estado,
            int horasCursada, DateTime ultimoPresentismo, decimal porcentajeAsistencia, string cursada, bool activo, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(int id, InscripcionMateria inscripcionMateria, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
