using SCM.ApiGateway.Aplicacion.DTO.Request;

namespace SCM.ApiGateway.Aplicacion.UseCasePorts.InputPort
{
    public interface IAutenticacionInputPort
    {
        Task Handle(AccesosRequestDto request);
    }
}
