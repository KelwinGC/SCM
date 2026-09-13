
using Microsoft.Extensions.DependencyInjection;
using SCM.ApiLoadProcess.Aplicacion.UseCasePorts.InputPort;

namespace SCM.ApiLoadProcess.Aplicacion.UseCase
{
    public static class DependencyConteiner
    {
        public static IServiceCollection AddUseCaseServicios(this IServiceCollection servicio)
        {
            servicio.AddTransient<ILoadProcessArchivoInputPort, LoadProcessArchivoUseCase>();
            //servicio.AddTransient<IInsertarTokenAccesoInputPort, InsertarTokenAcceso.InsertarTokenAcceso>();
            return servicio;
        }
    }
}
