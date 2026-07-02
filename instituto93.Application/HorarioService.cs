using instituto93.Data.Repositories;
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
    public class HorarioService : IHorarioService
    {
        private readonly IHorarioRepository _repo;

        public HorarioService(IHorarioRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<List<IHorario>> GetHorarios(CancellationToken cancellationToken = default)
        {
            var horarios = await _repo.GetAllAsync(cancellationToken);
            return horarios.ToList();
        }

        public async Task<IHorario?> GetHorarioById(int id, CancellationToken cancellationToken = default)
        {
            return await _repo.GetByIdAsync(id, cancellationToken);
        }

        public async Task<List<IHorario>> GetHorariosByCursoMateria(int cursoMateriaId, CancellationToken cancellationToken = default)
        {
            var horarios = await _repo.GetByCursoMateriaAsync(cursoMateriaId, cancellationToken);
            return horarios.ToList();
        }

        public async Task<int> CrearHorario(IHorario horario, CancellationToken cancellationToken = default)
        {
            if (horario == null)
                throw new ArgumentNullException(nameof(horario));

            if (horario.CursoMateriaId <= 0)
                throw new Exception("Debe seleccionar una materia del curso.");

            return await _repo.CreateAsync(horario, cancellationToken);
        }

        public async Task<bool> ActualizarHorario(int id, IHorario horario, CancellationToken cancellationToken = default)
        {
            if (horario == null)
                throw new ArgumentNullException(nameof(horario));

            if (id <= 0)
                throw new Exception("El id del horario no es válido.");

            if (horario.CursoMateriaId <= 0)
                throw new Exception("Debe seleccionar una materia del curso.");

            horario.HorarioId = id;

            return await _repo.UpdateAsync(horario, cancellationToken);
        }

        public async Task<bool> EliminarHorario(int id, CancellationToken cancellationToken = default)
        {
            if (id <= 0)
                throw new Exception("El id del horario no es válido.");

            return await _repo.DeleteAsync(id, cancellationToken);
        }
    }
}
