using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SCM.ApiAutenticacion.Dominio.Interface;
using SCM.ApiAutenticacion.Infraestructura.Repositorio.Contexts;
using SCM.ApiAutenticacion.Infraestructura.Repositorio.Interceptors;

namespace SCM.ApiAutenticacion.Infraestructura.Repositorio
{
    public static class DependencyConteiner
    {
        public static IServiceCollection AddRepositorio(this IServiceCollection servicio, IConfiguration configuration)
        {

            servicio.AddSingleton<DapperContext>();
            servicio.AddScoped<AuditableEntitySaveChangesInterceptor>();
            servicio.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("SCMConnection"),
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            servicio.AddScoped<IUserRepositorio, UserRepositorio>();
            return servicio;
        }
    }
}
