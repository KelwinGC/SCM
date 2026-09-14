using SCM.ApiGateway.Aplicacion.DTO.Response;
using SCM.ApiGateway.Aplicacion.UseCasePorts.OutputPort;
using SCM.ApiGateway.Framework.Presentador.Interfase;


namespace SCM.ApiGateway.Framework.Presentador.Json
{
    public class ControlScmJson: IControlOutputPort, IPresenterDataResponse<Json<ControlResponseDto>>
    {
        public Json<ControlResponseDto> Contenido { get; set; }

        public Task Handle(ResponseGenericoDto<ControlResponseDto> response)
        {
            Contenido = new Json<ControlResponseDto>()
            {
                codigo = response.Codigo,
                mensaje = response.Mensaje,
                data = response.Data
            };

            return Task.CompletedTask;
        }
    }
}
