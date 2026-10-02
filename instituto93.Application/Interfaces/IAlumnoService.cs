using instituto93.Domain.DTOs;
using instituto93.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace instituto93.Application
{
    //Lopez Melany
    public interface IAlumnoService
    {
        Task<List<AlumnoModelo>> GetAlumnosModelos(CancellationToken cancellationToken = default);

        Task<AlumnoModelo?> GetAlumnoByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<int> CreatePreinscripcionAsync(
            PreinscripcionDto preinscripcion,
            CancellationToken cancellationToken = default);
    }
}