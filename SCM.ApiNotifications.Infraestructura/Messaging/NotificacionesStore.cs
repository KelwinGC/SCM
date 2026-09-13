using System.Collections.Concurrent;
using SCM.Shared.Contracts;

namespace SCM.ApiNotifications.Infraestructura.Messaging;

/// <summary>
/// Este microservicio es PURO CONSUMIDOR: no expone ningun endpoint de
/// negocio para "crear" nada. El unico endpoint HTTP que tiene es de
/// solo lectura, para poder comprobar en la demo que el mensaje efectivamente
/// llego. En un caso real, aca normalmente se enviaria un email, un push,
/// se guardaria en una tabla de notificaciones en base de datos, etc.
/// </summary>
public class NotificacionesStore
{
    private readonly ConcurrentQueue<ArchivoProcesadoEvent> _notificaciones = new();
    private const int MaxItems = 100;

    public void Add(ArchivoProcesadoEvent notificacion)
    {
        _notificaciones.Enqueue(notificacion);
        while (_notificaciones.Count > MaxItems && _notificaciones.TryDequeue(out _)) { }
    }

    public IReadOnlyCollection<ArchivoProcesadoEvent> GetAll() => _notificaciones.ToArray();
}
