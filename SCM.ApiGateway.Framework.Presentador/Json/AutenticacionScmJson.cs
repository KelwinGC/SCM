using SCM.ApiGateway.Aplicacion.DTO.Response;
using SCM.ApiGateway.Aplicacion.UseCasePorts.OutputPort;
using SCM.ApiGateway.Framework.Presentador.Interfase;


namespace SCM.ApiGateway.Framework.Presentador.Json
{
    public class AutenticacionScmJson : IAutenticacionOutputPort, IPresenterDataResponse<Json<TokenAutenticacionResponseDto>>
    {
        public Json<TokenAutenticacionResponseDto> Contenido { get; set; }

        public Task Handle(ResponseGenericoDto<TokenAutenticacionResponseDto> response)
        {
            Contenido = new Json<TokenAutenticacionResponseDto>()
            {
                codigo = response.Codigo,
                mensaje = response.Mensaje,
                data = response.Data
            };

            return Task.CompletedTask;
        }
    }
}
