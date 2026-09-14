using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SCM.ApiGateway.Aplicacion.DTO.Request;
using SCM.ApiGateway.Aplicacion.DTO.Response;
using SCM.ApiGateway.Aplicacion.UseCasePorts.InputPort;
using SCM.ApiGateway.Aplicacion.UseCasePorts.OutputPort;
using SCM.ApiGateway.Framework.Presentador.Interfase;
using SCM.ApiGateway.Framework.Presentador.Json;

namespace SCM.ApiGateway.Controllers
{
    [Route("Gateway/Control")]
    [ApiController]
    [Authorize]
    public class ControlScmController : ControllerBase
    {
        private readonly IControlOutputPort _outputPort;
        private readonly IControlInputPort _inputPort;
        
        public ControlScmController(IControlOutputPort outputPort, IControlInputPort inputPort)
        {
            _outputPort = outputPort;
            _inputPort = inputPort;
        }

        [HttpPost]
        public async Task<Json<ControlResponseDto>> cargarArchivo(ControlRequestDto request)
        {
            await _inputPort.Handle(request);
            var respuesta = ((IPresenterDataResponse<Json<ControlResponseDto>>)_outputPort).Contenido;
            return respuesta;
        }
    }
}
