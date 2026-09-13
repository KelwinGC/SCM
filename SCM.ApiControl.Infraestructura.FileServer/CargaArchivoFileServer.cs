using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SCM.ApiControl.Dominio.Entidad;
using SCM.ApiControl.Dominio.Interface;

namespace SCM.ApiControl.Infraestructura.FileServer
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

        public async Task<CargaArchivo> SubirArchivo(IFormFile archivo, string seaweedFilerUrl)
        {
            try
            {
                using var form = new MultipartFormDataContent();
                using var streamContent = new StreamContent(archivo.OpenReadStream());

                // SeaweedFS espera que el campo del archivo se llame "file"
                form.Add(streamContent, "file", archivo.FileName);

                _logger.LogInformation($"Subiendo archivo '{archivo.FileName}' a SeaweedFS...");
                HttpResponseMessage response = await _httpClient.PostAsync(seaweedFilerUrl, form);


                if (response.IsSuccessStatusCode)
                {
                    string fullFilePath = $"{seaweedFilerUrl}{archivo.FileName}";
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("¡Subida exitosa!");
                    _logger.LogInformation($"Respuesta del servidor: {jsonResponse}");

                    var cargaArchivo = new CargaArchivo
                    {
                        NombreArchivo = archivo.FileName,
                        TamanoBytes = archivo.Length,
                        RutaArchivo = fullFilePath,//jsonResponse,
                        //FechaRegistro = DateTime.Now,
                        //Estado = "Pendiente"
                    };

                    return cargaArchivo;
                }
                else
                {
                    _logger.LogError($"Error al subir: {response.StatusCode}");
                    return new CargaArchivo { Estado = "Error", FechaRegistro = DateTime.Now };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocurrió un error: {ex.Message}");
                return new CargaArchivo { Estado = "Error", FechaRegistro = DateTime.Now };
            }
        }
    }
}
