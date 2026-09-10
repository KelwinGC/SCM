using SCM.ApiAutenticacion.Aplicacion.DTO.Request;
using SCM.ApiAutenticacion.Aplicacion.DTO.Response;
using SCM.ApiAutenticacion.Aplicacion.UseCasePorts.InputPort;
using SCM.ApiAutenticacion.Aplicacion.UseCasePorts.OutputPort;
using SCM.ApiAutenticacion.Framework.Presentador;
using Microsoft.AspNetCore.Mvc;

namespace SCM.ApiAutenticacion.Controllers
{
    [Route("Api")]
    [ApiController]
    public class AutenticacionController : Controller
    {
        private readonly ILogger<AutenticacionController> _logger;
        private readonly IGetAutenticacionInputPort _inputPort;
        private readonly IAutenticacionTokenOutputPort _outputPort;

        public AutenticacionController(ILogger<AutenticacionController> logger, IGetAutenticacionInputPort inputPort, IAutenticacionTokenOutputPort outputPort)
        {
            _logger = logger;
            _inputPort = inputPort;
            _outputPort = outputPort;
        }

        [HttpPost("Acceso")]
        public async Task<JSON<TokenDTO>> Token(GetAutenticacionDTO usuario)
        {
            await _inputPort.Handle(usuario);
            return ((IPresenteDataResponse<JSON<TokenDTO>>)_outputPort).Contenido;
        }
    }
}
