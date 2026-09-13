using Microsoft.Extensions.DependencyInjection;
using SCM.ApiLoadProcess.Aplicacion.UseCasePorts.OutputPort;

namespace SCM.ApiLoadProcess.Framework.Presentador
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddPresenters(this IServiceCollection services)
        {
            services.AddScoped<ILoadProcessArchivoOutputPort,LoadProcessArchivoJson>();
            return services;
        }
    }
}
