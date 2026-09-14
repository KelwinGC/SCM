using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SCM.ApiNotifications.Dominio.Entidad;
using SCM.ApiNotifications.Dominio.Interface;
using SCM.ApiNotifications.Infraestructura.Repositorio.Contexts;

namespace SCM.ApiNotifications.Infraestructura.Repositorio
{
    public class NotificationsRepositorio : INotificationsRepositorio
    {
        private readonly ILogger<NotificationsRepositorio> _logger;
        private readonly ApplicationDbContext _context;

        public NotificationsRepositorio(
            IConfiguration config,
            ILogger<NotificationsRepositorio> logger,
            ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<CargaArchivo> ObtenerCargaArchivoAsync(int idCarga)
        {
            try
            {
                var cargaArchivo = await _context.CargasArchivo.FindAsync(idCarga);
                if (cargaArchivo == null)
                {
                    _logger.LogWarning($"Carga de archivo no encontrada: {idCarga}");
                }
                return cargaArchivo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener carga de archivo: {idCarga}");
                throw;
            }
        }

        public async Task<CargaArchivo> ActualizarEstadoCargaArchivoAsync(int idCarga, string nuevoEstado)
        {
            try
            {
                var cargaArchivo = await _context.CargasArchivo.FindAsync(idCarga);
                if (cargaArchivo == null)
                {
                    _logger.LogWarning($"Carga de archivo no encontrada: {idCarga}");
                    return null;
                }

                cargaArchivo.Estado = nuevoEstado;
                cargaArchivo.FechaFin = DateTime.UtcNow;

                _context.CargasArchivo.Update(cargaArchivo);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Estado de carga actualizado: {idCarga} - Nuevo estado: {nuevoEstado}");
                return cargaArchivo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar estado de carga de archivo: {idCarga}");
                throw;
            }
        }



    }
}

