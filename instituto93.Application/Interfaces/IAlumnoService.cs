using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using instituto93.Domain.Models;

namespace instituto93.Application
{
    //Lopez Melany
    public interface IAlumnoService
    {
        Task<List<AlumnoModelo>> GetAlumnosModelos(CancellationToken cancellationToken = default);
    }
}