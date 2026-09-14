using SCM.ApiGateway.Aplicacion.DTO.Response.Services;

namespace SCM.ApiGateway.Infraestructura.WebServices.Interface
{
    public interface IAutenticacionScm
    {
        Task<AutenticacionScmResponse> Autenticar(string usuario, string clave);
    }
}
