using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SCM.ApiNotifications.Aplicacion.DTO.Request;
using SCM.ApiNotifications.Aplicacion.DTO.Response;
using SCM.ApiNotifications.Aplicacion.UseCasePorts.InputPort;
using SCM.ApiNotifications.Aplicacion.UseCasePorts.OutputPort;
using SCM.ApiNotifications.Framework.Presentador;

namespace SCM.ApiNotifications.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificacionController : ControllerBase
    {
        private readonly ILogger<NotificacionController> _logger;
        private readonly IConfiguration _configuration;
        private readonly INotificarInputPort _inputPort;
        private readonly INotificarOutputPort _outputPort;
        public NotificacionController(
            ILogger<NotificacionController> logger, 
            IConfiguration configuration, 
            INotificarInputPort inputPort, 
            INotificarOutputPort outputPort)
        {
            _logger = logger;
            _configuration = configuration;
            _inputPort = inputPort;
            _outputPort = outputPort;
        }


        [HttpPost]
        public async Task<JSON<ResponseNotificarDTO>> Notificar([FromBody] PeticionNotificarDTO request)
        {
            _logger.LogInformation("Notificar endpoint called.");
            await _inputPort.Handle(request);
            return ((IPresenteDataResponse<JSON<ResponseNotificarDTO>>)_outputPort).Contenido;

        }


        [HttpGet("test")]
        public IActionResult Test()
        {
            _logger.LogInformation("Test endpoint called.");
            return Ok(new { message = "NotificacionController is working!" });
        }
    }
}
