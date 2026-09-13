using System.Text.Json.Serialization;

namespace SCM.Shared.Contracts;

/// <summary>
/// Publicado por WebApi.Productor. Consumido por WebApi.Procesador
/// desde la cola "carga_masiva".
/// </summary>
public class CargaArchivoCreatedEvent : IntegrationEvent
{
    public int IdCarga { get; }
    public string RutaArchivo { get; }
    public string Usuario { get; }

    public CargaArchivoCreatedEvent(int idCarga, string rutaArchivo, string usuario)
    {
        IdCarga = idCarga;
        RutaArchivo = rutaArchivo;
        Usuario = usuario;
    }

    [JsonConstructor]
    public CargaArchivoCreatedEvent(Guid id, DateTime creationDate, int idCarga, string rutaArchivo, string usuario)
        : base(id, creationDate)
    {
        IdCarga = idCarga;
        RutaArchivo = rutaArchivo;
        Usuario = usuario;
    }
}
