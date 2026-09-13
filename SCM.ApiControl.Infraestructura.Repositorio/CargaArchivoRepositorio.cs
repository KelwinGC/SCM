using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SCM.ApiControl.Dominio.Entidad;
using SCM.ApiControl.Dominio.Interface;
using SCM.ApiControl.Infraestructura.Repositorio.Contexts;

namespace SCM.ApiControl.Infraestructura.Repositorio
{
    public class CargaArchivoRepositorio : ICargaArchivoRepositorio
    {
        private readonly ILogger<CargaArchivoRepositorio> _logger;
        private readonly ApplicationDbContext _context;

        public CargaArchivoRepositorio(
            IConfiguration config,
            ILogger<CargaArchivoRepositorio> logger,
            ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<CargaArchivo> RegistrarCargaArchivo(CargaArchivo cargaArchivo)
        {
            try
            {
                _context.CargasArchivo.Add(cargaArchivo);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Carga de archivo registrada: {cargaArchivo.IdCarga}");
                return cargaArchivo;
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
    }
}

