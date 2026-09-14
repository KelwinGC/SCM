using Microsoft.Extensions.DependencyInjection;
using SCM.ApiNotifications.Aplicacion.UseCasePorts.OutputPort;

namespace SCM.ApiNotifications.Framework.Presentador
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddPresenters(this IServiceCollection services)
        {
            services.AddScoped<INotificarOutputPort,NotificationJson>();
            return services;
        }
    }
}
