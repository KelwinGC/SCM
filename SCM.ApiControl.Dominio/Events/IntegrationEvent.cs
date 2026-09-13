using System.Text.Json.Serialization;

namespace SCM.ApiControl.Dominio.Events;

/// <summary>
/// Clase base para todos los eventos que viajan por el bus.
/// Todo evento tiene identidad propia y fecha de creacion,
/// independientemente del broker que se use por debajo.
/// </summary>
public abstract class IntegrationEvent
{
    public Guid Id { get; }
    public DateTime CreationDate { get; }

    protected IntegrationEvent()
    {
        Id = Guid.NewGuid();
        CreationDate = DateTime.UtcNow;
    }

    // Constructor usado por el deserializador al reconstruir el evento
    // que llego desde la cola (necesita preservar el Id/fecha originales).
    [JsonConstructor]
    protected IntegrationEvent(Guid id, DateTime creationDate)
    {
        Id = id;
        CreationDate = creationDate;
    }
}
