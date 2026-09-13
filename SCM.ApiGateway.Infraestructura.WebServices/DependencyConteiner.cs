using SCM.ApiGateway.Infraestructura.WebServices.Interface;
using SCM.ApiGateway.Infraestructura.WebServices.Servicios;
using Microsoft.Extensions.DependencyInjection;


namespace SCM.ApiGateway.Infraestructura.WebServices
{
    public static class DependencyConteiner
    {
        public static IServiceCollection AddWebServices(this IServiceCollection services)
        {
            //services.AddScoped<IConsultaPorNumeroDocumentoReniecIrma, ConsultaPorNumeroDocumentoReniecIrma>();
            services.AddScoped<IAutenticacionKeyScm, AutenticacionKeyScm>();
            return services;
 
        }
    }
}
