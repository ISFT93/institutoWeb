using instituto93.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instituto93.Application.Interfaces
{
    //Ibarra Valentino
    public interface ILibroActasService
    {
        Task<List<LibroActasModelo>> GetLibroActas(CancellationToken cancellationToken = default);
    }
}
