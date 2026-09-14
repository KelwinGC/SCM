using System.Text.Json.Serialization;

namespace SCM.ApiGateway.Aplicacion.DTO.Response.Services
{
    public class ControlScmResponse
    {
        public string? Codigo { get; set; }
        public string? Mensaje { get; set; }

        [JsonPropertyName("data")]
        public CargaArchivoResponse Data { get; set; }
    }

    public class CargaArchivoResponse
    {
        public int? IdCarga { get; set; }
        public string NombreArchivo { get; set; }
        public string Usuario { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Estado { get; set; }
        public long TamanoBytes { get; set; }      
        
        public string? RutaArchivo { get; set; }


    }
}
