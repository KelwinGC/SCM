using Microsoft.Extensions.DependencyInjection;
using SCM.ApiControl.Aplicacion.UseCasePorts.InputPort;
using SCM.ApiControl.Aplicacion.UseCasePorts.OutputPort;

namespace SCM.ApiControl.Framework.Presentador
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddPresenters(this IServiceCollection services)
        {
            services.AddScoped<ICargarArchivoOutputPort,CargaArchivoJson>();
            services.AddScoped<IEventTestOutputPort, EventTestJson>();
            return services;
        }
    }
}
