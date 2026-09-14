using MassTransit;
using SCM.Shared.Contracts;
using SCM.ApiNotifications.Infraestructura.Messaging;
using Microsoft.Extensions.Logging;
using SCM.ApiNotifications.Aplicacion.UseCasePorts.InputPort;
using SCM.ApiNotifications.Aplicacion.DTO.Request;

namespace SCM.ApiNotifications.Infraestructura;

public class ArchivoProcesadoEventConsumer : IConsumer<ArchivoProcesadoEvent>
{
    private readonly NotificacionesStore _store;
    private readonly ILogger<ArchivoProcesadoEventConsumer> _logger;
    private readonly INotificarInputPort _inputPort;

    public ArchivoProcesadoEventConsumer(
        NotificacionesStore store, 
        ILogger<ArchivoProcesadoEventConsumer> logger,
        INotificarInputPort inputPort
        )
    {
        _store = store;
        _logger = logger;
        _inputPort = inputPort;
    }

    public async Task Consume(ConsumeContext<ArchivoProcesadoEvent> context)
    {
        var message = context.Message;

        try
        {
            _logger.LogInformation($"[CONSUMER-NOTIFICATION] Procesando mensaje - IdCarga: {message.IdCarga}, Usuario: {message.Usuario}");

            // Usar el UseCase para procesar el archivo
            var peticion = new PeticionNotificarDTO
            {
                IdCarga = message.IdCarga,
                Usuario = message.Usuario
            };

            await _inputPort.Handle(peticion);

            _logger.LogInformation($"[CONSUMER-NOTIFICATION] Mensaje procesado exitosamente - IdCarga: {message.IdCarga}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"[CONSUMER-NOTIFICATION] Error procesando mensaje - IdCarga: {message.IdCarga}, Error: {ex.Message}");
            throw; // Reintentará el mensaje
        }
    }
}
