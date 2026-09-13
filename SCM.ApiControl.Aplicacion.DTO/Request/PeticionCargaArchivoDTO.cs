using Microsoft.AspNetCore.Http;

namespace SCM.ApiControl.Aplicacion.DTO.Request
{
    public class PeticionCargaArchivoDTO
    {
        public IFormFile Archivo { get; set; }
        public string Usuario { get; set; }
    }
}
