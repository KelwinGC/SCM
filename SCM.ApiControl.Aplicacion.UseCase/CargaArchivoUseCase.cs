using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SCM.ApiControl.Aplicacion.DTO.Request;
using SCM.ApiControl.Aplicacion.DTO.Response;
using SCM.ApiControl.Aplicacion.UseCasePorts.InputPort;
using SCM.ApiControl.Aplicacion.UseCasePorts.OutputPort;
using SCM.ApiControl.Dominio.Entidad;
//using SCM.ApiControl.Dominio.Events;
using SCM.Shared.EventBus.Abstractions;
using SCM.ApiControl.Dominio.Interface;
using SCM.Shared.Contracts;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SCM.ApiControl.Aplicacion.UseCase
{
    public class CargaArchivoUseCase : ICargarArchivoInputPort
    {
        private readonly ICargaArchivoRepositorio _cargaArchivoRepositorio;
        private readonly ICargaArchivoFileServer _cargaArchivoFileServer;
        private readonly IEventBus _eventBus;
        private readonly ICargarArchivoOutputPort _outputPort;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CargaArchivoUseCase> _logger;
        private readonly long _tamanoMaximoBytes;
        private readonly string _rutaAlmacenamiento;
        private readonly string[] _extensionesPermitidas = { ".xlsx", ".xls" };

        public CargaArchivoUseCase(
            ICargaArchivoRepositorio cargaArchivoRepositorio,
            ICargaArchivoFileServer cargaArchivoFileServer,
            IEventBus eventBus,
            ICargarArchivoOutputPort cargarArchivoOutputPort,
            IConfiguration configuration,
            ILogger<CargaArchivoUseCase> logger)
        {
            _cargaArchivoRepositorio = cargaArchivoRepositorio;
            _cargaArchivoFileServer = cargaArchivoFileServer;
            _eventBus = eventBus;   
            _configuration = configuration;
            _logger = logger;
            _outputPort = cargarArchivoOutputPort;

            var tamanoMaximoMB = int.TryParse(_configuration["ConfiguracionArchivos:TamanoMaximoMB"], out var result) ? result : 10;
            _tamanoMaximoBytes = tamanoMaximoMB * 1024 * 1024;

            _rutaAlmacenamiento = _configuration["ConfiguracionArchivos:RutaAlmacenamiento"]
                ?? Path.Combine(Directory.GetCurrentDirectory(), "Descargas", "ArchivosExcel");
        }

        public async Task Handle(PeticionCargaArchivoDTO peticion)
        {
            try
            {
                // Validar que se proporcionó un archivo
                if (peticion.Archivo == null || peticion.Archivo.Length == 0)
                {
                    _logger.LogWarning("Intento de carga sin archivo proporcionado");
                    throw new AbandonedMutexException($"304");
                }

                // Validar extensión del archivo
                var extension = Path.GetExtension(peticion.Archivo.FileName).ToLower();
                if (!_extensionesPermitidas.Contains(extension))
                {
                    _logger.LogWarning($"Extensión no permitida: {extension}");
                    throw new AbandonedMutexException($"305");
                }

                // Validar tamaño del archivo
                if (peticion.Archivo.Length > _tamanoMaximoBytes)
                {
                    var tamanoMaximoMB = _tamanoMaximoBytes / (1024 * 1024);
                    _logger.LogWarning($"Archivo excede tamaño máximo. Tamaño: {peticion.Archivo.Length}, Máximo: {_tamanoMaximoBytes}");
                    throw new AbandonedMutexException($"306");
                }

                var archivoGuardado = await _cargaArchivoFileServer.SubirArchivo(peticion.Archivo, _rutaAlmacenamiento);

                // Crear entidad de carga de archivo
                var cargaArchivo = new CargaArchivo
                {
                    NombreArchivo = archivoGuardado.NombreArchivo,
                    Usuario =  archivoGuardado.Usuario, 
                    FechaRegistro = DateTime.UtcNow,
                    Estado = "Pendiente",
                    TamanoBytes = archivoGuardado.TamanoBytes,
                    RutaArchivo = archivoGuardado.RutaArchivo
                };

                // Guardar registro en base de datos
                var resultado = await _cargaArchivoRepositorio.RegistrarCargaArchivo(cargaArchivo);

                //TODO: Enviar mensaje a cola carga_masiva de RabbitMQ
                /* Publicamos el evento */
                //var cargaArchivoCreatedEvent = new CargaArchivoCreatedEvent();
                //cargaArchivoCreatedEvent.IdCarga = resultado.IdCarga;
                //cargaArchivoCreatedEvent.RutaArchivo = resultado.RutaArchivo;
                //cargaArchivoCreatedEvent.Usuario = resultado.Usuario;

                //_mapper.Map<DiscountCreatedEvent>(discount);
                //_eventBus.Publish(cargaArchivoCreatedEvent);
                //await _eventBus.PublishAsync(cargaArchivoCreatedEvent);
                //var @event = new CargaArchivoCreatedIntegrationEvent(resultado.IdCarga, resultado.RutaArchivo, resultado.Usuario);
                var cargaArchivoCreatedEvent = new CargaArchivoCreatedEvent(resultado.IdCarga, resultado.RutaArchivo, resultado.Usuario);
                await _eventBus.PublishAsync(cargaArchivoCreatedEvent);

                var cargaArchivoDTO = new CargaArchivoDTO
                { 
                    IdCarga = resultado.IdCarga,
                    NombreArchivo = resultado.NombreArchivo,
                    Usuario = resultado.Usuario,
                    FechaRegistro = resultado.FechaRegistro,
                    Estado = resultado.Estado,
                    TamanoBytes = resultado.TamanoBytes,
                    RutaArchivo = resultado.RutaArchivo
                };

                _logger.LogInformation($"Archivo registrado con éxito. IdCarga: {resultado.IdCarga}");

                ResponseHeaderDTO responseHeaderDTO = new();
                CargaArchivoDTO data = new();
                responseHeaderDTO.Codigo = "300";
                responseHeaderDTO.Mensaje = _configuration["MCGS:300"];
                data.IdCarga = resultado.IdCarga;
                data.NombreArchivo = resultado.NombreArchivo;
                data.Usuario = resultado.Usuario;
                data.FechaRegistro = resultado.FechaRegistro;
                data.Estado = resultado.Estado;
                data.TamanoBytes = resultado.TamanoBytes;
                data.RutaArchivo = resultado.RutaArchivo;

                await _outputPort.Handle(responseHeaderDTO, data);
            }
            catch (Exception ex)
            {
                ResponseHeaderDTO responseHeader = new();
                CargaArchivoDTO data = new();
                switch (ex.Message)
                {
                    case "304":
                        responseHeader.Codigo = "304";
                        responseHeader.Mensaje = _configuration["MSGS:304"];
                        break;
                    case "305":
                        responseHeader.Codigo = "305";
                        responseHeader.Mensaje = _configuration["MSGS:305"];
                        break;
                    case "306":
                        responseHeader.Codigo = "306";
                        responseHeader.Mensaje = _configuration["MSGS:306"];
                        break;
                    default:
                        responseHeader.Codigo = "307";
                        responseHeader.Mensaje = _configuration["MSGS:307"];
                        _logger.LogError("APICONTROL-307 CargarArchivo - Handle \n" + ex.ToString());
                        break;
                }
                await _outputPort.Handle(responseHeader, data);
            }
        }
    }
}
