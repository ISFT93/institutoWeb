using instituto93.Data.Repositories.Interfaces;
using instituto93.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace instituto93.Data.Repositories
{
    public class InscripcionMateriaRepository : IInscripcionMateriaRepository
    {
        private readonly Conexion _conexion;

        public InscripcionMateriaRepository(Conexion conexion)
        {
            _conexion = conexion ?? throw new ArgumentNullException(nameof(conexion));
        }

        public async Task<IEnumerable<InscripcionMateria>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var lista = new List<InscripcionMateria>();
            const string sql = "SELECT a.AlumnoId, cac.Estado, cac.Cursada FROM Alumnos AS a " +
                "INNER JOIN AlumnosCarreras AS ac ON a.AlumnoId = ac.AlumnoId " +
                "INNER JOIN CursadaAlumnoCarreras AS cac ON cac.AlumnoCarreraId = ac.AlumnoCarreraId";                         

            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                while (await reader.ReadAsync(cancellationToken))
                {
                    lista.Add(new InscripcionMateria
                    {
                        cursadaAlumnoId = reader.GetInt32(reader.GetOrdinal("AlumnoId")),
                        estado = reader.GetString(reader.GetOrdinal("Estado")),
                        cursada = reader.GetString(reader.GetOrdinal("Cursada"))
                    });
                }
            }
            finally
            {
                _conexion.Close();                
            }

            return lista;
        }

        public async Task<InscripcionMateria?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var lista = new List<InscripcionMateria>();
            const string sql = "SELECT a.AlumnoId, cac.Estado, cac.Cursada FROM Alumnos AS a " +
                "INNER JOIN AlumnosCarreras AS ac ON a.AlumnoId = ac.AlumnoId " +
                "INNER JOIN CursadaAlumnoCarreras AS cac ON cac.AlumnoCarreraId = ac.AlumnoCarreraId " +
                "WHERE a.AlumnoId = @AlumnoId";

            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@AlumnoId", id);
                using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                if (await reader.ReadAsync(cancellationToken))
                {
                    return new InscripcionMateria
                    {
                        cursadaAlumnoId = reader.GetInt32(reader.GetOrdinal("AlumnoId")),
                        estado = reader.GetString(reader.GetOrdinal("Estado")),
                        cursada = reader.GetString(reader.GetOrdinal("Cursada"))
                    };
                }
                return null;
            }
            finally
            {
                _conexion.Close();
            }
        }

        public async Task<int> CreateAsync(int alumnoCarreraId,int cursadaId,int anioCicloLectivo,string estado,int horasCursada,DateTime ultimoPresentismo, decimal porcentajeAsistencia,string cursada,bool activo, CancellationToken cancellationToken = default)
        {
            const string sql = "INSERT INTO CursadaAlumnoCarreras VALUES(@AlumnoCarreraId," +
                "@CursadaId,@AnioCicloLectivo,@Estado,@HorasCursadas,@UltimoPresentimos,@PorcentajeAsistencia,@Cursada,@Activo)";

            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@AlumnoCarreraId",alumnoCarreraId);
                cmd.Parameters.AddWithValue("@CursadaId",cursadaId);
                cmd.Parameters.AddWithValue("@AnioCicloLectivo",anioCicloLectivo);
                cmd.Parameters.AddWithValue("@Estado",estado);
                cmd.Parameters.AddWithValue("@HorasCursadas",horasCursada);
                cmd.Parameters.AddWithValue("@UltimoPresentimos",ultimoPresentismo);
                cmd.Parameters.AddWithValue("@Cursada",cursada);
                cmd.Parameters.AddWithValue("@Activo",activo);
                var result = await cmd.ExecuteScalarAsync(cancellationToken);
                return Convert.ToInt32(result);
            }
            finally
            {
                _conexion.Close();
            }

        }

        public async Task<bool> UpdateAsync(int id, InscripcionMateria inscripcionMateria, CancellationToken cancellationToken = default)
        {
            if (inscripcionMateria == null) throw new ArgumentNullException(nameof(inscripcionMateria));          
            string sql = "UPDATE CursadaAlumnoCarreras SET Estado =  @Estado, Cursada = @Cursada WHERE CursadaAlumnoCarreraId = @CursadaAlumnoCarreraId";

            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@Estado", inscripcionMateria.estado);
                cmd.Parameters.AddWithValue("@Cursada", inscripcionMateria.cursada);
                cmd.Parameters.AddWithValue("@CursadaAlumnoCarreraId", id);
                var rows = await cmd.ExecuteNonQueryAsync(cancellationToken);
                return rows > 0;
            }
            finally
            {
                _conexion.Close();
            }
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            const string sql = "DELETE FROM CursadaAlumnoCarreras WHERE CursadaAlumnoCarreraId = @CursadaAlumnoCarreraId";

            try
            {
                await _conexion.OpenAsync(cancellationToken);
                using var cmd = _conexion.Conector.CreateCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@CursadaAlumnoCarreraId", id);
                var rows = await cmd.ExecuteNonQueryAsync(cancellationToken);
                return rows > 0;
            }
            finally
            {
                _conexion.Close();
            }
        }

        
    }
}
