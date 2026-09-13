using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SCM.ApiLoadProcess.Dominio.Interface;
using SCM.ApiLoadProcess.Infraestructura.Repositorio.Contexts;
using SCM.ApiLoadProcess.Infraestructura.Repositorio.Interceptors;

namespace SCM.ApiLoadProcess.Infraestructura.Repositorio
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
            servicio.AddScoped<ILoadProcessRepositorio, LoadProcessRepositorio>();
            return servicio;
        }
    }
}

