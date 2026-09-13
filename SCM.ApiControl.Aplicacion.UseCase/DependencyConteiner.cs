
using Microsoft.Extensions.DependencyInjection;
using SCM.ApiControl.Aplicacion.UseCasePorts.InputPort;

namespace SCM.ApiControl.Aplicacion.UseCase
{
    public static class DependencyConteiner
    {
        public static IServiceCollection AddUseCaseServicios(this IServiceCollection servicio)
        {
            servicio.AddTransient<ICargarArchivoInputPort, CargaArchivoUseCase>();
            servicio.AddTransient<IEventTestInputPort, EventTestUseCase>();
            //servicio.AddTransient<IInsertarTokenAccesoInputPort, InsertarTokenAcceso.InsertarTokenAcceso>();
            return servicio;
        }
    }
}
