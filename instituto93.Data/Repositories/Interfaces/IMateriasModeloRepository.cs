using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using instituto93.Domain.Models;

namespace instituto93.Data.Repositories.Interfaces
{
    public interface IMateriasModeloRepository
    {
        Task<IEnumerable<MateriasModelo>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<MateriasModelo?> GetByIdAsync(int MateriaId, CancellationToken cancellationToken = default);
        Task<int> CreateAsync(MateriasModelo materia, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(MateriasModelo materia, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int MateriaId, CancellationToken cancellationToken = default);
    }
}
