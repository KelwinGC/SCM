using MassTransit;
using Microsoft.Extensions.Logging;
using SCM.ApiLoadProcess.Aplicacion.DTO.Request;
using SCM.ApiLoadProcess.Aplicacion.UseCasePorts.InputPort;
using SCM.Shared.Contracts;

namespace SCM.ApiLoadProcess.Infraestructura
{
    public class CargaArchivoCreatedEventConsumer : IConsumer<CargaArchivoCreatedEvent>
    {
        private readonly ILogger<CargaArchivoCreatedEventConsumer> _logger;
        private readonly ILoadProcessArchivoInputPort _loadProcessArchivoUseCase;

        public CargaArchivoCreatedEventConsumer(ILogger<CargaArchivoCreatedEventConsumer> logger,ILoadProcessArchivoInputPort loadProcessArchivoInputPort)
        {
            _logger = logger;
            _loadProcessArchivoUseCase = loadProcessArchivoInputPort;
        }

        public async Task Consume(ConsumeContext<CargaArchivoCreatedEvent> context)
        {
            var message = context.Message;

            try
            {
                _logger.LogInformation($"[CONSUMER] Procesando mensaje - IdCarga: {message.IdCarga}, Usuario: {message.Usuario}, Ruta: {message.RutaArchivo}");

                // Usar el UseCase para procesar el archivo
                var peticion = new PeticionLoadProcessArchivoDTO
                {
                    IdCarga = message.IdCarga,
                    Usuario = message.Usuario
                };

                await _loadProcessArchivoUseCase.Handle(peticion);

                _logger.LogInformation($"[CONSUMER] Mensaje procesado exitosamente - IdCarga: {message.IdCarga}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"[CONSUMER] Error procesando mensaje - IdCarga: {message.IdCarga}, Error: {ex.Message}");
                throw; // Reintentará el mensaje
            }
        }
    }
}
