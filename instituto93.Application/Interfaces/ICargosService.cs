using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using instituto93.Domain.Models;

namespace instituto93.Application
{
    public interface ICargosService
    {
        Task<List<CargosModelo>> GetCargos(CancellationToken cancellationToken = default);
    }
}