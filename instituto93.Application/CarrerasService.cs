using instituto93.Application.Interfaces;
using instituto93.Data.Repositories;
using instituto93.Data.Repositories.Interfaces;
using instituto93.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instituto93.Application
{
    public class CarrerasService : ICarrerasService
    {
        private readonly ICarrerasRepository _repo;
        public CarrerasService(ICarrerasRepository repo)
        {
            _repo = repo ?? throw new System.ArgumentNullException(nameof(repo));
        }
        public async Task<List<CarrerasModelo>> GetCarreras(CancellationToken cancellationToken = default)
        {
            var carreras = await _repo.GetAllAsync(cancellationToken);
            return carreras.ToList();
        }
    }
}
