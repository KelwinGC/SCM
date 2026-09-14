using Microsoft.AspNetCore.Http;

namespace SCM.ApiGateway.Aplicacion.DTO.Request
{
    public class ControlRequestDto
    {
        public IFormFile Archivo { get; set; }
        public string Usuario { get; set; }
    }
}
