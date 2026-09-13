using Microsoft.Extensions.DependencyInjection;
using SCM.ApiGateway.Transversal.Interface;
using SCM.ApiGateway.Transversal.Service;

namespace SCM.ApiGateway.Transversal
{
    public static class DependencyConteiner
    {
        public static IServiceCollection AddTransversalServicios(this IServiceCollection services)
        {
            services.AddScoped<ISecurity, Security>();
            return services;
        }
    }
}
