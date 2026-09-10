using SCM.ApiAutenticacion.Aplicacion.DTO.Response;

namespace SCM.ApiAutenticacion.Aplicacion.UseCasePorts.OutputPort
{
    public interface IAutenticacionTokenOutputPort
    {
        Task Handle(ResponseHeaderDTO requestHeader, TokenDTO Token);
    }
}
