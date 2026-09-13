namespace SCM.ApiControl.Aplicacion.DTO.Response
{
    public class CargaArchivoDTO
    {
        public int IdCarga { get; set; }
        public string NombreArchivo { get; set; }
        public string Usuario { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Estado { get; set; }
        public long TamanoBytes { get; set; }
        public string RutaArchivo { get; set; }
    }
}
