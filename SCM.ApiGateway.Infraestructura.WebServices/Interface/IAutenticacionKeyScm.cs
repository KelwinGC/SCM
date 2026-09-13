using SCM.ApiGateway.Infraestructura.WebServices.Response;

namespace SCM.ApiGateway.Infraestructura.WebServices.Interface
{
    public interface IAutenticacionKeyScm
    {
        Task<AutenticacionKeyScmResponse> AutenticacionScm(string usuario, string clave);
    }
}
