using instituto93.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using instituto93.Data.Repositories.Interfaces;
using instituto93.Domain.Models;

namespace instituto93.Application
{
    //Ibarra Valentino
    public class LibroActasService : ILibroActasService
    {
        private readonly ILibrosActasRepository _repo;
        public LibroActasService(ILibrosActasRepository repo)
        {
            _repo = repo ?? throw new System.ArgumentNullException(nameof(repo));
        }
        public async Task<List<LibroActasModelo>> GetLibroActas(CancellationToken cancellationToken = default)
        {
            var libroActas = await _repo.GetAllAsync(cancellationToken);
            return libroActas.ToList();
        }
    }
}
