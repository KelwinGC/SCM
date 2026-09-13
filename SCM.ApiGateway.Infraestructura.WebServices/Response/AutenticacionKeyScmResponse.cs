using System.Text.Json.Serialization;

namespace SCM.ApiGateway.Infraestructura.WebServices.Response
{
    public class AutenticacionKeyScmResponse
    {
        public string? Codigo { get; set; }
        public string? Mensaje { get; set; }

        [JsonPropertyName("data")]
        public TokenAcceso Data { get; set; }
    }

    public class TokenAcceso
    {
        public string? Token { get; set; }
        //public Guid? SessionId { get; set; }
        public string? SessionId { get; set; }

    }

}
