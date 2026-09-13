
namespace SCM.ApiControl.Dominio.Events
{
    public class CargaArchivoCreatedIntegrationEvent: IntegrationEvent
    {
        public int IdCarga { get; set; }
        public string RutaArchivo { get; set; }
        public string Usuario { get; set; }

        public CargaArchivoCreatedIntegrationEvent(int idCarga, string rutaArchivo, string usuario)
        {
            IdCarga = idCarga;
            RutaArchivo = rutaArchivo;
            Usuario = usuario;
        }

        public CargaArchivoCreatedIntegrationEvent(Guid id, DateTime creationDate, int idCarga, string rutaArchivo, string usuario)
            : base(id, creationDate)
        {
            IdCarga = idCarga;
            RutaArchivo = rutaArchivo;
            Usuario = usuario;
        }
    }
}
