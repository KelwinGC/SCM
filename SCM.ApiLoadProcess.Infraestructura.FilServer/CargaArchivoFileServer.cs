using Microsoft.Extensions.Logging;
using SCM.ApiLoadProcess.Dominio.Entidad;
using SCM.ApiLoadProcess.Dominio.Interface;

namespace SCM.ApiLoadProcess.Infraestructura.FileServer
{
    public class CargaArchivoFileServer : ICargaArchivoFileServer
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CargaArchivoFileServer> _logger;

        public CargaArchivoFileServer(HttpClient httpClient, ILogger<CargaArchivoFileServer> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<CargaArchivo> DescargarArchivo(string seaweedFilerUrl)
        {
            try
            {
                // Construir URL de descarga en SeaweedFS
                //string urlDescarga = $"{seaweedFilerUrl.TrimEnd('/')}/{nombreArchivo}";

                //_logger.LogInformation($"Descargando archivo '{nombreArchivo}' desde SeaweedFS en: {urlDescarga}");

                HttpResponseMessage response = await _httpClient.GetAsync(seaweedFilerUrl);

                if (response.IsSuccessStatusCode)
                {
                    byte[] contenidoArchivo = await response.Content.ReadAsByteArrayAsync();

                    _logger.LogInformation($"¡Descarga exitosa! Tamaño: {contenidoArchivo.Length} bytes");

                    var cargaArchivo = new CargaArchivo
                    {
                        NombreArchivo = seaweedFilerUrl,
                        TamanoBytes = contenidoArchivo.Length,
                        RutaArchivo = seaweedFilerUrl,
                        //ContenidoArchivo = contenidoArchivo,
                        FechaRegistro = DateTime.Now,
                        Estado = "Completado"
                    };

                    return cargaArchivo;
                }
                else
                {
                    _logger.LogError($"Error al descargar: {response.StatusCode} - {response.ReasonPhrase}");
                    return new CargaArchivo { Estado = "Error", FechaRegistro = DateTime.Now };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrió un error al descargar: {ex.Message}");
                return new CargaArchivo { Estado = "Error", FechaRegistro = DateTime.Now };
            }
        }
    }
}
