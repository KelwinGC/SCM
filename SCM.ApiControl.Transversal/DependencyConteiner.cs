using Microsoft.Extensions.DependencyInjection;
using SCM.ApiControl.Transversal.Interface;
using SCM.ApiControl.Transversal.Service;

namespace SCM.ApiControl.Transversal
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
