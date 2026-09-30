using SCM.ApiGateway.Aplicacion.DTO.Mappings;
using SCM.ApiGateway.Aplicacion.UseCasePorts.InputPort;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper; 

namespace SCM.ApiGateway.Aplicacion.UseCase
{
    public static class DependencyConteiner
    {
        public static IServiceCollection AddUseCaseServicios(this IServiceCollection services)
        {
            // A partir de la version 15 (ya tiene licencia comercial pero se puede usar como community), pide configuracion de mapeo, por eso se agrega el cfg => { } aunque no se use 
            services.AddAutoMapper(cfg => { }, typeof(EntitiApiRestToDtoProfile).Assembly);
            services.AddScoped<IAutenticacionInputPort, AutenticacionScmUseCase>();
            services.AddScoped<IControlInputPort, ControlScmUseCase>();

            return services;
        }
    }
}
