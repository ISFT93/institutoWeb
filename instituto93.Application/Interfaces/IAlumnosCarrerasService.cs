using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using instituto93.Domain.Models;

namespace instituto93.Application.Interfaces
{
    // Acevedo Cecilia
    public interface IAlumnosCarrerasService
    {
        // Obtener todos los registros en forma de Lista
        Task<List<AlumnosCarreras>> GetAllAsync(CancellationToken cancellationToken = default);

        // Obtener uno solo por su ID
        Task<AlumnosCarreras?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        // Crear una nueva relación alumno-carrera
        Task<int> CreateAsync(AlumnosCarreras alumnoCarrera, CancellationToken cancellationToken = default);

        // Actualizar los datos de una relación existente
        Task<bool> UpdateAsync(AlumnosCarreras alumnoCarrera, CancellationToken cancellationToken = default);

        // Eliminar un registro por su ID
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
