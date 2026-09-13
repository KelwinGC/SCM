using SCM.ApiLoadProcess.Dominio.Base;

namespace SCM.ApiLoadProcess.Dominio.Entidad
{
    public class CargaArchivo : BaseAuditableEntity
    {
        public int IdCarga { get; set; }
        public string NombreArchivo { get; set; }
        public string Usuario { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Estado { get; set; }
        public DateTime? FechaFin { get; set; }
        public long TamanoBytes { get; set; }
        public string RutaArchivo { get; set; }
    }
}