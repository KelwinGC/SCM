using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using SCM.ApiLoadProcess.Dominio.Entidad;
using SCM.ApiLoadProcess.Dominio.Interface;
using SCM.ApiLoadProcess.Infraestructura.Repositorio.Contexts;

namespace SCM.ApiLoadProcess.Infraestructura.Repositorio
{
    public class LoadProcessRepositorio : ILoadProcessRepositorio
    {
        private readonly ILogger<LoadProcessRepositorio> _logger;
        private readonly ApplicationDbContext _context;

        public LoadProcessRepositorio(
            IConfiguration config,
            ILogger<LoadProcessRepositorio> logger,
            ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<int> RegistrarDataProcesada(List<DataProcesada> listDataProcesada)
        {
            try
            {
                await _context.DataProcesadas.AddRangeAsync(listDataProcesada);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Carga de archivo registrada: {listDataProcesada.Count}");
                return listDataProcesada.Count  ;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar carga de archivo");
                throw;
            }

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

