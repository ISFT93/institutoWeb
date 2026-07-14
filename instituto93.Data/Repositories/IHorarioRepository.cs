using instituto93.Domain.Interfaces;
using instituto93.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instituto93.Data.Repositories
{
    //Mourdad ignacio
    public interface IHorarioRepository
    {
        Task<IEnumerable<IHorario>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<IHorario?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<IEnumerable<IHorario>> GetByCursoMateriaAsync(int cursoMateriaId, CancellationToken cancellationToken = default);

        Task<int> CreateAsync(IHorario horario, CancellationToken cancellationToken = default);

        Task<bool> UpdateAsync(IHorario horario, CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
