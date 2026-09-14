using System.Text.Json.Serialization;

namespace SCM.ApiGateway.Aplicacion.DTO.Response.Services
{
    public class AutenticacionScmResponse
    {
        public string? Codigo { get; set; }
        public string? Mensaje { get; set; }

        [JsonPropertyName("data")]
        public TokenAcceso Data { get; set; }
    }

    public class TokenAcceso
    {
        public string? Token { get; set; }

    }

}
