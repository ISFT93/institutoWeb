using instituto93.Application.Interfaces;
using instituto93.Data.Repositories.Interfaces;
using instituto93.Domain.Models;

namespace instituto93.Application
{
    public class InscripcionMateriaService : IInscripcionMateriaService
    {
        private readonly IInscripcionMateriaRepository _repo;

        public InscripcionMateriaService(IInscripcionMateriaRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<List<InscripcionMateria>> GetInscripcionMateriaAsync(CancellationToken cancellationToken = default)
        {
            var result = await _repo.GetAllAsync(cancellationToken);
            return result.ToList();
        }

        public Task<InscripcionMateria?> GetInscripcionMateriaByIdAsync(int id, CancellationToken cancellationToken = default)
            => _repo.GetByIdAsync(id, cancellationToken);

        public Task<int> CreateInscripcionMateriaAsync(int cursadaAlumnoCarreraId, int alumnoCarreraId, int cursadaId, int anioCicloLectivo, string estado,
            int horasCursada, DateTime ultimoPresentismo, decimal porcentajeAsistencia, string cursada, bool activo, CancellationToken cancellationToken = default)
            => _repo.CreateAsync(cursadaAlumnoCarreraId, alumnoCarreraId, cursadaId, anioCicloLectivo, estado,
            horasCursada, ultimoPresentismo, porcentajeAsistencia, cursada, activo, cancellationToken);

        public Task<bool> UpdateInscripcionMateriaAsync(InscripcionMateria inscripcionMateria, CancellationToken cancellationToken = default)
            => _repo.UpdateAsync(inscripcionMateria, cancellationToken);

        public Task<bool> DeleteInscripcionMateriaAsync(int id, CancellationToken cancellationToken = default)
            => _repo.DeleteAsync(id, cancellationToken);

    }
}
