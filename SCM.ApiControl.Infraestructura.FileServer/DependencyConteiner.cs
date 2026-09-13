using Microsoft.Extensions.DependencyInjection;
using SCM.ApiControl.Dominio.Interface;

namespace SCM.ApiControl.Infraestructura.FileServer
{
    public static class DependencyConteiner
    {
        public static IServiceCollection AddFileServer(this IServiceCollection servicio)
        {
            servicio.AddHttpClient();
            servicio.AddScoped<ICargaArchivoFileServer, CargaArchivoFileServer>();
            return servicio;
        }
    }
}

