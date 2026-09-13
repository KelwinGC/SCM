using SCM.ApiLoadProcess.Dominio.Entidad;

namespace SCM.ApiLoadProcess.Dominio.Interface
{
    public interface ICargaArchivoFileServer
    {
        //Task<CargaArchivo> SubirArchivo(IFormFile archivo, string seaweedFilerUrl);
        Task<CargaArchivo> DescargarArchivo(string seaweedFilerUrl);

    }
}
