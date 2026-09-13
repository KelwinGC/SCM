using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SCM.ApiControl.Dominio.Interface;
using SCM.ApiControl.Infraestructura.Repositorio.Contexts;
using SCM.ApiControl.Infraestructura.Repositorio.Interceptors;

namespace SCM.ApiControl.Infraestructura.Repositorio
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
            servicio.AddScoped<ICargaArchivoRepositorio, CargaArchivoRepositorio>();
            return servicio;
        }
    }
}

