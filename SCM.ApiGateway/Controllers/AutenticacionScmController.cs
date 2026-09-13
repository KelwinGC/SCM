using SCM.ApiGateway.Aplicacion.DTO.Request;
using SCM.ApiGateway.Aplicacion.DTO.Response;
using SCM.ApiGateway.Aplicacion.UseCasePorts.InputPort;
using SCM.ApiGateway.Aplicacion.UseCasePorts.OutputPort;
using SCM.ApiGateway.Framework.Presentador.Interfase;
using SCM.ApiGateway.Framework.Presentador.Json;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SCM.ApiGateway.Controllers
{
    [Route("Gateway")]
    [ApiController]
    public class AutenticacionScmController : ControllerBase
    {
        private readonly IAutenticacionOutputPort _outputPort;
        private readonly IAutenticacionInputPort _inputPort;

        public AutenticacionScmController(IAutenticacionOutputPort outputPort, IAutenticacionInputPort inputPort) =>
            (_outputPort,_inputPort) = (outputPort,inputPort);

        [HttpPost("Autenticacion")]
        public async Task<Json<TokenAutenticacionResponseDto>> Autenticacion(AccesosRequest request)
        {
            await _inputPort.Handle(request);
            var respuesta = ((IPresenterDataResponse<Json<TokenAutenticacionResponseDto>>)_outputPort).Contenido;
            return respuesta;
        }

    }
}
