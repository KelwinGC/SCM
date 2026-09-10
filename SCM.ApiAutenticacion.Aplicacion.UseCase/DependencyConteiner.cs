
using Microsoft.Extensions.DependencyInjection;
using SCM.ApiAutenticacion.Aplicacion.UseCasePorts.InputPort;

namespace SCM.ApiAutenticacion.Aplicacion.UseCase
{
    public static class DependencyConteiner
    {
        public static IServiceCollection AddUseCaseServicios(this IServiceCollection servicio)
        {
            servicio.AddTransient<IGetAutenticacionInputPort, GetAutenticacion>();
            //servicio.AddTransient<IInsertarTokenAccesoInputPort, InsertarTokenAcceso.InsertarTokenAcceso>();
            return servicio;
        }
    }
}
