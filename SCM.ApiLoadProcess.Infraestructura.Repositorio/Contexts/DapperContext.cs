using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SCM.ApiLoadProcess.Infraestructura.Repositorio.Contexts
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("SCMConnection");
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
