using Microsoft.Extensions.DependencyInjection;
using SCM.ApiAutenticacion.Aplicacion.UseCasePorts.InputPort;
using SCM.ApiAutenticacion.Aplicacion.UseCasePorts.OutputPort;

namespace SCM.ApiAutenticacion.Framework.Presentador
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddPresenters(this IServiceCollection services)
        {
            services.AddScoped<IAutenticacionTokenOutputPort,AutenticacionTokenJson>();
            return services;
        }
    }
}
