using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using instituto93.Domain.Models;

namespace instituto93.Application
{
    public interface ILocalidadService
    {
        Task<List<Localidad>> GetLocalidades(CancellationToken cancellationToken = default);
    }
}