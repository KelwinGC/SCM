using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SCM.ApiNotifications.Dominio.Interface;
using SCM.ApiNotifications.Infraestructura.Repositorio.Contexts;
using SCM.ApiNotifications.Infraestructura.Repositorio.Interceptors;

namespace SCM.ApiNotifications.Infraestructura.Repositorio
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
            servicio.AddScoped<INotificationsRepositorio, NotificationsRepositorio>();
            return servicio;
        }
    }
}

