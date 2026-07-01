using instituto93.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instituto93.Application
{
    public interface ICursoService
    {
        Task<List<CursoModelo>> GetCursos(CancellationToken cancellationToken = default);

    }
}
