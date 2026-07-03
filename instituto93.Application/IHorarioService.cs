using instituto93.Domain.Interfaces;
using instituto93.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace instituto93.Application
{
    //Mourdad ignacio
    public interface IHorarioService
    {
        Task<List<IHorario>> GetHorarios(CancellationToken cancellationToken = default);

        Task<IHorario?> GetHorarioById(int id, CancellationToken cancellationToken = default);

        Task<List<IHorario>> GetHorariosByCursoMateria(int cursoMateriaId, CancellationToken cancellationToken = default);

        Task<int> CrearHorario(IHorario horario, CancellationToken cancellationToken = default);

        Task<bool> ActualizarHorario(int id, IHorario horario, CancellationToken cancellationToken = default);

        Task<bool> EliminarHorario(int id, CancellationToken cancellationToken = default);
    }
}
