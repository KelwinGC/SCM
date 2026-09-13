using SCM.ApiControl.Dominio.Entidad;

namespace SCM.ApiControl.Dominio.Interface
{
    public interface ICargaArchivoRepositorio
    {
        Task<CargaArchivo> RegistrarCargaArchivo(CargaArchivo cargaArchivo);
        Task<CargaArchivo> ObtenerCargaArchivoAsync(int idCarga);
    }
}
