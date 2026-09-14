using SCM.ApiGateway.Aplicacion.DTO.Response;


namespace SCM.ApiGateway.Aplicacion.UseCasePorts.OutputPort
{
    public interface IControlOutputPort
    {
        Task Handle(ResponseGenericoDto<ControlResponseDto> response);

    }
}
