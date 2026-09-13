using System.Text.Json.Serialization;

namespace SCM.Shared.Contracts;

/// <summary>
/// Clase base de todo evento que viaja entre microservicios.
///
/// CLAVE: esta clase (y las que heredan de ella) vive en un proyecto de
/// librería COMPARTIDO, referenciado por los 3 microservicios como
/// ProjectReference/PackageReference. Ningun microservicio define su propia
/// copia de estos eventos. MassTransit identifica los mensajes por su tipo
/// .NET (namespace + nombre); si cada microservicio tuviera su propia clase
/// "igual pero distinta", el consumidor nunca reconoceria el mensaje del
/// publicador aunque llegara a la cola correcta.
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

    [JsonConstructor]
    protected IntegrationEvent(Guid id, DateTime creationDate)
    {
        Id = id;
        CreationDate = creationDate;
    }
}
