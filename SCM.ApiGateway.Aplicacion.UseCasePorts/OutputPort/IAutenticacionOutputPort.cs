using SCM.ApiGateway.Aplicacion.DTO.Response;

namespace SCM.ApiGateway.Aplicacion.UseCasePorts.OutputPort
{
    public interface IAutenticacionOutputPort
    {
        Task Handle(ResponseGenericoDto<TokenAutenticacionResponseDto> response);
    }
}
