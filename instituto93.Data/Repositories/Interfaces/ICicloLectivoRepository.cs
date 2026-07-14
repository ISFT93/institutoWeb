using instituto93.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using instituto93.Domain.Models;

namespace instituto93.Data.Repositories.Interfaces
{
    public interface ICicloLectivoRepository
    {

        Task<IEnumerable<CicloLectivoModelo>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<CicloLectivoModelo?> GetByAnioAsync(int anioLectivo, CancellationToken cancellationToken = default);

        Task<int> CreateAsync(CicloLectivoModelo cicloLectivo, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(CicloLectivoModelo cicloLectivo, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int anioLectivo, CancellationToken cancellationToken = default);
    }
}
