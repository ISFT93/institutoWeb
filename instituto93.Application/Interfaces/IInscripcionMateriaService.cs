using instituto93.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instituto93.Application.Interfaces
{
    public interface IInscripcionMateriaService
    {
        Task<List<InscripcionMateria>> GetInscripcionMateriaAsync(CancellationToken cancellationToken = default);
        Task<InscripcionMateria?> GetInscripcionMateriaByIdAsync(int id, CancellationToken cancellationToken = default);        
        Task<int> CreateInscripcionMateriaAsync(int alumnoCarreraId, int cursadaId, int anioCicloLectivo, string estado,
            int horasCursada, DateTime ultimoPresentismo, decimal porcentajeAsistencia, string cursada, bool activo, CancellationToken cancellationToken = default);
        Task<bool> UpdateInscripcionMateriaAsync(InscripcionMateria inscripcionMateria, CancellationToken cancellationToken = default);
        Task<bool> DeleteInscripcionMateriaAsync(int id, CancellationToken cancellationToken = default);

    }
}
