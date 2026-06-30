using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using instituto93.Domain.Models;

namespace instituto93.Application
{
    public interface ICargosService
    {
        Task<List<Cargo>> GetCargos(CancellationToken cancellationToken = default);
        Task<Cargo> GetByIdCargos(int id, CancellationToken cancellationToken = default);
        Task<int> CreateCargo (Cargo cargo, CancellationToken cancellationToken = default);
        Task<bool> UpdateCargo (Cargo cargo, CancellationToken cancellationToken = default);
        Task<bool> DeleteCargo (int id, CancellationToken cancellationToken = default);
    }
}