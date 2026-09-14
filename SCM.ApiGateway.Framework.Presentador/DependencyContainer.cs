using SCM.ApiGateway.Aplicacion.UseCasePorts.OutputPort;
using SCM.ApiGateway.Framework.Presentador.Json;
using Microsoft.Extensions.DependencyInjection;


namespace SCM.ApiGateway.Framework.Presentador
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddPresenters(this IServiceCollection service) 
        {
            //service.AddScoped<IConsultarPorNumeroDeDocumentoReniecOutputPort, ConsultaPorNumeroDeDocumentoReniecJson>();
            service.AddScoped<IAutenticacionOutputPort, AutenticacionScmJson>();
            service.AddScoped<IControlOutputPort, ControlScmJson>();
            return service;
        }
    }
}
