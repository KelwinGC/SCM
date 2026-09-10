using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SCM.ApiAutenticacion.Dominio.Entidad;
using SCM.ApiAutenticacion.Dominio.Interface;
using SCM.ApiAutenticacion.Infraestructura.Repositorio.Contexts;
using System.Data;

namespace SCM.ApiAutenticacion.Infraestructura.Repositorio
{
    public class UserRepositorio : IUserRepositorio
    {
        readonly ILogger<UserRepositorio> _logger;
        private readonly DapperContext _context;


        public UserRepositorio(IConfiguration config, ILogger<UserRepositorio> logger, DapperContext context)
        {

            _logger = logger;
            _context = context;

        }

        public async Task<User> Authenticate(string userName, string password)
        {
            using (var connection = _context.CreateConnection())
            {
                var query = "sp_UsersGetByUserAndPassword";
                var parameters = new DynamicParameters();
                parameters.Add("UserName", userName);
                parameters.Add("Password", password);

                var user = await connection.QuerySingleOrDefaultAsync<User>(query, param: parameters, commandType: CommandType.StoredProcedure);
                return user;
            }
        }

    }
}
