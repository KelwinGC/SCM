using SCM.ApiGateway.Aplicacion.DTO.Mappings;
using SCM.ApiGateway.Aplicacion.UseCase;
//using SCM.ApiGateway.Aplicacion.UseCase.Reniec;
using SCM.ApiGateway.Aplicacion.UseCasePorts.InputPort;
using Microsoft.Extensions.DependencyInjection;


namespace SCM.ApiGateway.Aplicacion.UseCase
{
    public static class DependencyConteiner
    {
        
        public static IServiceCollection AddUseCaseServicios(this IServiceCollection services)
        {
            //services.AddScoped<IConsultarPorNumeroDeDocumentoReniecInputPort, ConsultarPorNumeroDeDocumentoReniec>();
            services.AddAutoMapper(typeof(EntitiApiRestToDtoProfile));
            services.AddScoped<IAutenticacionInputPort, AutenticacionScmUseCase>();
            services.AddScoped<IControlInputPort, ControlScmUseCase>();


            return services;
        }

    }
}
