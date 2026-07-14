using System;
using System.Collections.Generic;
using System.Linq; // Necesario para usar el .ToList()
using System.Threading;
using System.Threading.Tasks;
using instituto93.Application.Interfaces; // Para que encuentre tu IAlumnosCarrerasService
using instituto93.Data.Repositories.Interfaces; // Para que encuentre tu IAlumnosCarrerasRepository
using instituto93.Domain.Models;

namespace instituto93.Application
{
    // Acevedo Cecilia
    public class AlumnosCarrerasService : IAlumnosCarrerasService
    {
        private readonly IAlumnosCarrerasRepository _repo;

        // El constructor recibe la interfaz del repositorio (Inyección de Dependencias) 
        //y la asigna a un campo privado para usarlo en los métodos
        public AlumnosCarrerasService(IAlumnosCarrerasRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        // OBTENER TODOS REGISTROS 
        public async Task<List<AlumnosCarreras>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            // Le pide al repositorio que traiga todos los registros
            var resultado = await _repo.GetAllAsync(cancellationToken);

            // Lo convertimos a una Lista real antes de devolverlo
            return resultado.ToList();
        }

        // OBTENER UNO SOLO POR ID 
        public async Task<AlumnosCarreras?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            // Busca en el repositorio por ID
            return await _repo.GetByIdAsync(id, cancellationToken);
        }

        // CREAR REGISTRO 
        public async Task<int> CreateAsync(AlumnosCarreras alumnoCarrera, CancellationToken cancellationToken = default)
        {
            // Le pide el objeto completo al repositorio para que lo inserte
            return await _repo.CreateAsync(alumnoCarrera, cancellationToken);
        }

        // ACTUALIZAR REGISTRO
        public async Task<bool> UpdateAsync(AlumnosCarreras alumnoCarrera, CancellationToken cancellationToken = default)
        {
            // Le pasa el objeto modificado al repositorio
            return await _repo.UpdateAsync(alumnoCarrera, cancellationToken);
        }

        // ELIMINAR REGISTRO
        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            // Le pide al repositorio que borre la fila con este ID
            return await _repo.DeleteAsync(id, cancellationToken);
        }
    }
}