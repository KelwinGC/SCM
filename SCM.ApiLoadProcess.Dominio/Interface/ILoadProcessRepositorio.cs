using SCM.ApiLoadProcess.Dominio.Entidad;
using SCM.ApiLoadProcess.Dominio.Entidad;

namespace SCM.ApiLoadProcess.Dominio.Interface
{
    public interface ILoadProcessRepositorio
    {
        Task<int> RegistrarDataProcesada(List<DataProcesada> listDataProcesada);
        Task<CargaArchivo> ObtenerCargaArchivoAsync(int idCarga);
        Task<CargaArchivo> ActualizarEstadoCargaArchivoAsync(int idCarga, string nuevoEstado);
    }
}
