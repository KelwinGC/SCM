
using Microsoft.Extensions.DependencyInjection;
using SCM.ApiNotifications.Aplicacion.UseCasePorts.InputPort;

namespace SCM.ApiNotifications.Aplicacion.UseCase
{
    public static class DependencyConteiner
    {
        public static IServiceCollection AddUseCaseServicios(this IServiceCollection servicio)
        {
            servicio.AddTransient<INotificarInputPort, NotificarUseCase>();
            //servicio.AddTransient<IInsertarTokenAccesoInputPort, InsertarTokenAcceso.InsertarTokenAcceso>();
            return servicio;
        }
    }
}
