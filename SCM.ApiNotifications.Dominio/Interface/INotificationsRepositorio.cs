using SCM.ApiNotifications.Dominio.Entidad;

namespace SCM.ApiNotifications.Dominio.Interface
{
    public interface INotificationsRepositorio
    {
        Task<CargaArchivo> ObtenerCargaArchivoAsync(int idCarga);
        Task<CargaArchivo> ActualizarEstadoCargaArchivoAsync(int idCarga, string nuevoEstado);

    }
}
