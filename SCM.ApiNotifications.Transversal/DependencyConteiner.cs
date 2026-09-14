using Microsoft.Extensions.DependencyInjection;
using SCM.ApiNotifications.Transversal.Interface;
using SCM.ApiNotifications.Transversal.Service;

namespace SCM.ApiNotifications.Transversal
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
