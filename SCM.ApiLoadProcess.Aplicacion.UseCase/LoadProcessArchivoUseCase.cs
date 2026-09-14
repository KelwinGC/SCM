using ExcelDataReader;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SCM.ApiLoadProcess.Aplicacion.DTO.Request;
using SCM.ApiLoadProcess.Aplicacion.DTO.Response;
using SCM.ApiLoadProcess.Aplicacion.UseCasePorts.InputPort;
using SCM.ApiLoadProcess.Aplicacion.UseCasePorts.OutputPort;
using SCM.ApiLoadProcess.Dominio.Entidad;
using SCM.ApiLoadProcess.Dominio.Interface;
using SCM.Shared.Contracts;
using SCM.Shared.EventBus.Abstractions;

using System.Data;

namespace SCM.ApiLoadProcess.Aplicacion.UseCase
{
    public class LoadProcessArchivoUseCase : ILoadProcessArchivoInputPort
    {
        private readonly ILoadProcessRepositorio _loadProcessRepositorio;
        private readonly ICargaArchivoFileServer _cargaArchivoFileServer;
        private readonly IEventBus _eventBus;
        private readonly ILoadProcessArchivoOutputPort _outputPort;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoadProcessArchivoUseCase> _logger;
        private readonly HttpClient _httpClient;

        public LoadProcessArchivoUseCase(
            ILoadProcessRepositorio loadProcessRepositorio, 
            ICargaArchivoFileServer cargaArchivoFileServer, 
            IEventBus eventBus, 
            ILoadProcessArchivoOutputPort outputPort, 
            IConfiguration configuration, 
            ILogger<LoadProcessArchivoUseCase> logger,
            HttpClient httpClient)
        {
            _loadProcessRepositorio = loadProcessRepositorio;
            _cargaArchivoFileServer = cargaArchivoFileServer;
            _eventBus = eventBus;
            _outputPort = outputPort;
            _configuration = configuration;
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task Handle(PeticionLoadProcessArchivoDTO peticion)
        {
            CargaArchivo? cargaArchivo = null;
            var responseHeader = new ResponseHeaderDTO();
            
            try
            {              

                _logger.LogInformation($"Iniciando procesamiento de archivo. IdCarga: {peticion.IdCarga}, Usuario: {peticion.Usuario}");

                //Obtener datos de CargaArchivo 
                _logger.LogInformation($"Obteniendo datos de CargaArchivo desde la base de datos. IdCarga: {peticion.IdCarga}");
                cargaArchivo = await _loadProcessRepositorio.ObtenerCargaArchivoAsync(peticion.IdCarga);

                if (cargaArchivo == null)
                {
                    throw new InvalidOperationException($"No se pudo obtener datos de CargaArchivo desde la base de datos. IdCarga: {peticion.IdCarga}");
                }
                _logger.LogInformation($"Archivo obtenido exitosamente: {cargaArchivo.NombreArchivo}");

                cargaArchivo = await _loadProcessRepositorio.ActualizarEstadoCargaArchivoAsync(cargaArchivo.IdCarga, "En proceso");
                _logger.LogInformation($"En procesamiento. IdCarga: {peticion.IdCarga}");

                _logger.LogInformation("Procesando y validando datos del archivo");
                var numeroRegistros = await ProcesarDatos(cargaArchivo, peticion.Usuario);
                if (numeroRegistros == 0)   { 
                    throw new InvalidOperationException($"No se pudo cargar datos del ArchivoCarga en la base de datos.  IdCarga: {peticion.IdCarga}");
                } 

                cargaArchivo = await _loadProcessRepositorio.ActualizarEstadoCargaArchivoAsync(cargaArchivo.IdCarga, "Cargado");
                _logger.LogInformation($"Procesamiento completado exitosamente. IdCarga: {peticion.IdCarga}");

                //Publicar evento de éxito
                await PublicarEventoExito(cargaArchivo, numeroRegistros);

                // Actualizar estado CargaArchivo en BD
                cargaArchivo = await _loadProcessRepositorio.ActualizarEstadoCargaArchivoAsync(cargaArchivo.IdCarga, "Finalizado");
                _logger.LogInformation($"Procesamiento completado exitosamente. IdCarga: {peticion.IdCarga}");

                responseHeader.Codigo = "200";
                responseHeader.Mensaje = "Archivo procesado exitosamente";

                var cargaArchivoDTO = new CargaArchivoDTO
                {
                    IdCarga = cargaArchivo.IdCarga,
                    NombreArchivo = cargaArchivo.NombreArchivo,
                    Usuario = cargaArchivo.Usuario,
                    FechaRegistro = cargaArchivo.FechaRegistro,
                    Estado = cargaArchivo.Estado,
                    TamanoBytes = cargaArchivo.TamanoBytes,
                    RutaArchivo = cargaArchivo.RutaArchivo,
                    FechaFin = cargaArchivo.FechaFin
                };

                await _outputPort.Handle(responseHeader, cargaArchivoDTO);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning($"Validación de datos fallida: {ex.Message}");
                
                if (cargaArchivo != null)
                {
                    cargaArchivo.Estado = "Error - Validación";
                    PublicarEventoError(cargaArchivo, ex.Message);
                }

                responseHeader.Codigo = "400";
                responseHeader.Mensaje = $"Error en validación: {ex.Message}";
                
                await NotificarError(responseHeader, cargaArchivo);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError($"Error de operación: {ex.Message}");
                
                if (cargaArchivo != null)
                {
                    cargaArchivo.Estado = "Error - Operación";
                    PublicarEventoError(cargaArchivo, ex.Message);
                }

                responseHeader.Codigo = "500";
                responseHeader.Mensaje = $"Error al procesar archivo: {ex.Message}";
                
                await NotificarError(responseHeader, cargaArchivo);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error inesperado durante el procesamiento: {ex.Message}. Stack: {ex.StackTrace}");
                
                if (cargaArchivo != null)
                {
                    cargaArchivo.Estado = "Error - Sistema";
                    //cargaArchivo.FechaFin = DateTime.Now;
                    PublicarEventoError(cargaArchivo, ex.Message);
                }

                responseHeader.Codigo = "500";
                responseHeader.Mensaje = "Error inesperado durante el procesamiento del archivo";
                
                await NotificarError(responseHeader, cargaArchivo);
                //throw;
            }
        }

        private async Task<int> ProcesarDatos(CargaArchivo cargaArchivo, string usuario)
        {           

            try
            {
                if (cargaArchivo == null)
                {
                    throw new ArgumentException("El archivo no puede ser nulo");
                }
                // 1. Descargar el archivo desde la URL
                using var response = await _httpClient.GetAsync(cargaArchivo.RutaArchivo, HttpCompletionOption.ResponseHeadersRead);

                // Validar que la respuesta sea exitosa (Status Code 200 OK)
                response.EnsureSuccessStatusCode();

                using var networkStream = await response.Content.ReadAsStreamAsync();

                // 2. Copiar a MemoryStream para permitir la lectura con "Seek" (requerido por archivos .xlsx)
                using var memoryStream = new MemoryStream();
                await networkStream.CopyToAsync(memoryStream);
                memoryStream.Position = 0; // Rebobinar al inicio del flujo

                var registros = new List<DataProcesada>();

                // 3. Procesar el archivo con ExcelDataReader desde el MemoryStream
                using (var reader = ExcelReaderFactory.CreateReader(memoryStream))
                {
                    var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }
                    });

                    DataTable tabla = result.Tables[0];

                    foreach (DataRow fila in tabla.Rows)
                    {
                        
                        string periodo = fila["Periodo"]?.ToString();
                        string? codigoProducto = fila["CodigoProducto"]?.ToString()?.Trim();
                        if (string.IsNullOrWhiteSpace(codigoProducto)) continue; //codigoProducto = null;

                        string? descripcion = fila["Descripcion"]?.ToString()?.Trim();
                        int cantidad = int.TryParse(fila["Cantidad"]?.ToString(), out int c) ? c : 0;

                        registros.Add(new DataProcesada
                        {
                            Periodo = periodo,
                            CodigoProducto = codigoProducto,
                            Descripcion = descripcion,
                            Cantidad = cantidad,
                            IdCarga  = cargaArchivo.IdCarga
                        });
                    }
                }

                // 4. Guardar los registros en SQL Server mediante EF Core
                var numeroRegistros = await _loadProcessRepositorio.RegistrarDataProcesada(registros);
                _logger.LogInformation($"Datos procesados correctamente para archivo: {cargaArchivo.NombreArchivo}");
                
                return numeroRegistros;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al procesar datos del archivo: {ex.Message}");
                throw new InvalidOperationException($"No se pudieron procesar los datos: {ex.Message}", ex);
            }
        }

        private async Task PublicarEventoExito(CargaArchivo cargaArchivo, int numeroRegistros)
        {
            try
            {
              ArchivoProcesadoEvent resultado = new ArchivoProcesadoEvent(
              idCarga: cargaArchivo.IdCarga,
              usuario:cargaArchivo.Usuario,
              exitoso: true,
              mensaje: "Archivo procesado correctamente.",
              registrosProcesados: numeroRegistros);

               await _eventBus.PublishAsync(resultado);
                _logger.LogInformation($"Evento de éxito publicado para IdCarga: {cargaArchivo.IdCarga}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al publicar evento de éxito: {ex.Message}");
            }
        }

        private async Task PublicarEventoError(CargaArchivo cargaArchivo, string mensajeError)
        {
            try
            {

               ArchivoProcesadoEvent resultado = new ArchivoProcesadoEvent(
               idCarga: cargaArchivo.IdCarga,
               usuario: cargaArchivo.Usuario,
               exitoso: false,
               mensaje: $"Error procesando el archivo: {mensajeError}",
               registrosProcesados: 0);

                await _eventBus.PublishAsync(resultado);
                _logger.LogInformation($"Evento de error publicado para IdCarga: {cargaArchivo.IdCarga}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al publicar evento de error: {ex.Message}");
            }
        }

        private async Task NotificarError(ResponseHeaderDTO responseHeader, CargaArchivo? cargaArchivo)
        {
            try
            {
                var cargaArchivoDTO = new CargaArchivoDTO
                {
                    IdCarga = cargaArchivo?.IdCarga ?? 0,
                    NombreArchivo = cargaArchivo?.NombreArchivo ?? "Desconocido",
                    Usuario = cargaArchivo?.Usuario ?? "Sistema",
                    FechaRegistro = cargaArchivo?.FechaRegistro ?? DateTime.Now,
                    Estado = cargaArchivo?.Estado ?? "Error",
                    TamanoBytes = cargaArchivo?.TamanoBytes ?? 0,
                    RutaArchivo = cargaArchivo?.RutaArchivo ?? string.Empty
                };

                await _outputPort.Handle(responseHeader, cargaArchivoDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al notificar error en output port: {ex.Message}");
            }
        }
    }
}
