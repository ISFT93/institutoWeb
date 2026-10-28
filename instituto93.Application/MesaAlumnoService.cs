using instituto93.Application.Interfaces;
using instituto93.Data.Repositories.Interfaces;

namespace instituto93.Application
{
    public class MesaAlumnoService : IMesaAlumnoService
    {
        private readonly IMesasFinalesAlumnosRepository _repo;
        public MesaAlumnoService(IMesasFinalesAlumnosRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<int> InscribirAsync(int alumnoId, List<int> mesaFinalIds, CancellationToken cancellationToken = default)
        {
            if (alumnoId <= 0) throw new ArgumentException("Alumno inválido.", nameof(alumnoId));
            if (mesaFinalIds == null || mesaFinalIds.Count == 0) return 0;

            var ids = mesaFinalIds.Where(x => x > 0).Distinct().ToList();
            if (ids.Count == 0) return 0;

            var yaInscritos = await _repo.GetInscritosMesaIdsAsync(alumnoId, ids, cancellationToken);
            var aInscribir = ids.Except(yaInscritos).ToList();
            if (aInscribir.Count == 0) return 0;

            return await _repo.InscribirAsync(alumnoId, aInscribir, cancellationToken);
        }
    }
}
