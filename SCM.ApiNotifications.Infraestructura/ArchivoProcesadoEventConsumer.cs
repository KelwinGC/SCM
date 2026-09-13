using MassTransit;
using SCM.Shared.Contracts;
using SCM.ApiNotifications.Infraestructura.Messaging;
using Microsoft.Extensions.Logging;

namespace SCM.ApiNotifications.Infraestructura;

public class ArchivoProcesadoEventConsumer : IConsumer<ArchivoProcesadoEvent>
{
    private readonly NotificacionesStore _store;
    private readonly ILogger<ArchivoProcesadoEventConsumer> _logger;

    public ArchivoProcesadoEventConsumer(NotificacionesStore store, ILogger<ArchivoProcesadoEventConsumer> logger)
    {
        _store = store;
        _logger = logger;
    }

    public Task Consume(ConsumeContext<ArchivoProcesadoEvent> context)
    {
        var @event = context.Message;
        _store.Add(@event);

        if (@event.Exitoso)
        {
            _logger.LogInformation(
                "Notificar a {Usuario}: la carga {IdCarga} se proceso OK ({Registros} registros).",
                @event.Usuario, @event.IdCarga, @event.RegistrosProcesados);
        }
        else
        {
            _logger.LogWarning(
                "Notificar a {Usuario}: la carga {IdCarga} fallo. Motivo: {Mensaje}",
                @event.Usuario, @event.IdCarga, @event.Mensaje);
        }

        // Aca en un caso real: enviar email/push/SMS, guardar en tabla de notificaciones, etc.

        return Task.CompletedTask;
    }
}
