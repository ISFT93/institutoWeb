using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using instituto93.Data.Repositories;
using instituto93.Data.Repositories.Interfaces;
using instituto93.Domain.Models;
using System.Linq;
namespace instituto93.Application
{
    public class CicloLectivoService : ICicloLectivoService
    {
        private readonly ICicloLectivoRepository _repo;

        public CicloLectivoService(ICicloLectivoRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public async Task<List<CicloLectivoModelo>> GetCiclosLectivosAsync(CancellationToken cancellationToken = default)
        {
            var ciclos = await _repo.GetAllAsync(cancellationToken);
            return ciclos.ToList(); 
        }

        public async Task<CicloLectivoModelo?> GetByAnioAsync(int anioLectivo, CancellationToken cancellationToken = default)
        {
            return await _repo.GetByAnioAsync(anioLectivo, cancellationToken);
        }

        public async Task<int> CreateAsync(CicloLectivoModelo cicloLectivo, CancellationToken cancellationToken = default)
        {
            if (cicloLectivo == null) throw new ArgumentNullException(nameof(cicloLectivo));
            return await _repo.CreateAsync(cicloLectivo, cancellationToken);
        }

        public async Task<bool> UpdateAsync(CicloLectivoModelo cicloLectivo, CancellationToken cancellationToken = default)
        {
            if (cicloLectivo == null) throw new ArgumentNullException(nameof(cicloLectivo));
            return await _repo.UpdateAsync(cicloLectivo, cancellationToken);
        }

        public async Task<bool> DeleteAsync(int anioLectivo, CancellationToken cancellationToken = default)
        {
            return await _repo.DeleteAsync(anioLectivo, cancellationToken);
        }
    }
}