using instituto93.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instituto93.Data.Repositories.Interfaces
{
    public interface ICarrerasRepository
    {
        Task<IEnumerable<CarrerasModelo>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<CarrerasModelo?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<int> CreateAsync(CarrerasModelo carreras, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(CarrerasModelo carreras, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
