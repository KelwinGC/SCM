using Microsoft.Extensions.DependencyInjection;
using SCM.ApiLoadProcess.Dominio.Interface;

namespace SCM.ApiLoadProcess.Infraestructura.FileServer
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddFileServer(this IServiceCollection servicio)
        {
            servicio.AddHttpClient();
            servicio.AddScoped<ICargaArchivoFileServer, CargaArchivoFileServer>();
            return servicio;
        }
    }
}

