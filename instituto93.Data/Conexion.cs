using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;

namespace instituto93.Data
{
    public class Conexion(IConfiguration configuration)
    {
        private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection");

        //public void Conectar()
        //{
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        connection.Open();
                
        //    }
        //}
    }
}
