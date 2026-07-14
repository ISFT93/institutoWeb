using instituto93.Data.Repositories;
using instituto93.Domain.Interfaces;
using instituto93.Domain.Models;

namespace instituto93.Application
{
    //Looez Melany
    public class AlumnoService : IAlumnoService
    {
        private readonly IAlumnoRepository _repo;
        public AlumnoService(IAlumnoRepository repo)
        {
            _repo = repo ?? throw new System.ArgumentNullException(nameof(repo));
        }
        public async Task<List<AlumnoModelo>> GetAlumnosModelos(CancellationToken cancellationToken = default)
        {
            var alumnosModelos = await _repo.GetAllAsync(cancellationToken);
            return alumnosModelos.ToList();
        }

    }
}