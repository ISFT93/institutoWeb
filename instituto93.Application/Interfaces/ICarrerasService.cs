using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using instituto93.Domain.Models;

namespace instituto93.Application.Interfaces
{
    public interface ICarrerasService
    {
        Task<List<CarrerasModelo>> GetCarreras(CancellationToken cancellationToken = default);
    }
}
