using SCM.ApiGateway.Aplicacion.DTO.Request;

namespace SCM.ApiGateway.Aplicacion.UseCasePorts.InputPort
{
    public interface IControlInputPort
    {
        Task Handle(ControlRequestDto request);

    }
}
