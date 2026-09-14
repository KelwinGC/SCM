using SCM.ApiGateway.Aplicacion.DTO.Request;
using SCM.ApiGateway.Aplicacion.DTO.Response.Services;

namespace SCM.ApiGateway.Infraestructura.WebServices.Interface
{
    public interface IControlScm
    {
        //Task<ControlScmResponse> CargarArchivo(string usuario, string clave);
        Task<ControlScmResponse> CargarArchivo(ControlRequestDto request);

    }
}
