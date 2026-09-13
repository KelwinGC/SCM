
namespace SCM.ApiControl.Dominio.Events
{
    public class CargaArchivoCreatedEvent
    {
        public int IdCarga { get; set; }
        public string RutaArchivo { get; set; }
        public string Usuario { get; set; }
    }
}
