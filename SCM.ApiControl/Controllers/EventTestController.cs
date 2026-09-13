using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SCM.ApiControl.Aplicacion.DTO.Request;
using SCM.ApiControl.Aplicacion.DTO.Response;
using SCM.ApiControl.Aplicacion.UseCasePorts.InputPort;
using SCM.ApiControl.Aplicacion.UseCasePorts.OutputPort;
using SCM.ApiControl.Framework.Presentador;

namespace SCM.ApiControl.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    [Authorize]
    public class EventTestController : ControllerBase
    {
        private readonly ILogger<EventTestController> _logger;
        private readonly IEventTestInputPort _inputPort;
        private readonly IEventTestOutputPort _outputPort;

        public EventTestController(IEventTestInputPort inputPort, IEventTestOutputPort outputPort, ILogger<EventTestController> logger)
        {
            _inputPort = inputPort;
            _outputPort = outputPort;
            _logger = logger;
        }

        [HttpPost]
        public async Task<JSON<ResponseEventTestDTO>> Publish([FromBody] PeticionEventTestDTO request, CancellationToken cancellationToken)
        {

            await _inputPort.Handle(request);
            return ((IPresenteDataResponse<JSON<ResponseEventTestDTO>>)_outputPort).Contenido;

        }

    }
}
