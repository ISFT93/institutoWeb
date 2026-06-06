using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace instituto93.Data
{
    public class Conexion : IDisposable
    {
        public SqlConnection Conector { get; private set; }
        private readonly string _connectionString;

        public Conexion(IConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            _connectionString = configuration.GetConnectionString("InstiDb")
                ?? throw new InvalidOperationException("Falta la cadena de conexión 'InstiDb' en la configuración.");
            Conector = new SqlConnection(_connectionString);
        }

        public void Open()
        {
            if (Conector.State != ConnectionState.Open)
                Conector.Open();
        }

        public Task OpenAsync(CancellationToken cancellationToken = default)
        {
            if (Conector.State == ConnectionState.Open)
                return Task.CompletedTask;
            return Conector.OpenAsync(cancellationToken);
        }

        public void Close()
        {
            if (Conector.State != ConnectionState.Closed)
                Conector.Close();
        }

        public void Dispose()
        {
            Conector?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
