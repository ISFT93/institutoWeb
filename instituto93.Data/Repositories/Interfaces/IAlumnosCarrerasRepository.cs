using instituto93.Domain.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace instituto93.Data.Repositories.Interfaces
{
    // Acevedo Cecilia
    //no es una clase, es una Interfaz (te das cuenta por la I del nombre)
    //Se usa "public interface" para que sea un contrato válido y visible.
    public interface IAlumnosCarrerasRepository
    {
            // Traer todos los registros de AlumnosCarreras
            Task<IEnumerable<AlumnosCarreras>> GetAllAsync(CancellationToken cancellationToken = default);

            // Buscar un registro específico por su ID
            Task<AlumnosCarreras?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

            // Crear un nuevo registro
            Task<int> CreateAsync(AlumnosCarreras alumnoCarrera, CancellationToken cancellationToken = default);

            // Actualizar un registro existente
            Task<bool> UpdateAsync(AlumnosCarreras alumnoCarrera, CancellationToken cancellationToken = default);

            // Borrar un registro por su ID
            Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
