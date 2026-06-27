using instituto93.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace instituto93.Application.Interfaces
{
    public interface IMateriasModeloService
    {
        Task<List<MateriasModelo>> GetMateriasModelo(CancellationToken cancellationToken = default);
    }
}
