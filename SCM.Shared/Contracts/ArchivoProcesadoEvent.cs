using System.Text.Json.Serialization;

namespace SCM.Shared.Contracts;

/// <summary>
/// Publicado por WebApi.Procesador luego de procesar la carga.
/// Consumido por WebApi.Notificaciones desde la cola "notificaciones".
/// </summary>
public class ArchivoProcesadoEvent : IntegrationEvent
{
    public int IdCarga { get; }
    public string Usuario { get; }
    public bool Exitoso { get; }
    public string Mensaje { get; }
    public int RegistrosProcesados { get; }

    public ArchivoProcesadoEvent(int idCarga, string usuario, bool exitoso, string mensaje, int registrosProcesados)
    {
        IdCarga = idCarga;
        Usuario = usuario;
        Exitoso = exitoso;
        Mensaje = mensaje;
        RegistrosProcesados = registrosProcesados;
    }

    [JsonConstructor]
    public ArchivoProcesadoEvent(
        Guid id, DateTime creationDate, int idCarga, string usuario,
        bool exitoso, string mensaje, int registrosProcesados)
        : base(id, creationDate)
    {
        IdCarga = idCarga;
        Usuario = usuario;
        Exitoso = exitoso;
        Mensaje = mensaje;
        RegistrosProcesados = registrosProcesados;
    }
}
