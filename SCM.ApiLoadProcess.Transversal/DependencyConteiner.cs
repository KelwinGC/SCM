using Microsoft.Extensions.DependencyInjection;
using SCM.ApiLoadProcess.Transversal.Interface;
using SCM.ApiLoadProcess.Transversal.Service;

namespace SCM.ApiLoadProcess.Transversal
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
