using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using instituto93.Domain.Models;

namespace instituto93.Data.Repositories.Interfaces
{
    //Ibarra Valentino
    public interface ILibrosActasRepository
    {
        Task<IEnumerable<LibroActasModelo>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<LibroActasModelo?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CreateAsync(LibroActasModelo libroacta, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(LibroActasModelo libroacta, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
