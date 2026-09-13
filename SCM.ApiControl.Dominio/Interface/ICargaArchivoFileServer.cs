using Microsoft.AspNetCore.Http;
using SCM.ApiControl.Dominio.Entidad;

namespace SCM.ApiControl.Dominio.Interface
{
    public interface ICargaArchivoFileServer
    {
        Task<CargaArchivo> SubirArchivo(IFormFile archivo, string seaweedFilerUrl);
    }
}
