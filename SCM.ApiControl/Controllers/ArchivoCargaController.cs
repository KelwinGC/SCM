using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SCM.ApiControl.Aplicacion.DTO.Request;
using SCM.ApiControl.Aplicacion.DTO.Response;
using SCM.ApiControl.Aplicacion.UseCasePorts.InputPort;
using SCM.ApiControl.Aplicacion.UseCasePorts.OutputPort;
using SCM.ApiControl.Framework.Presentador;

namespace SCM.ApiControl.Controllers
{
    [Route("Api")]
    [ApiController]
    [Authorize]
    public class ArchivoCargaController : ControllerBase
    {
        private readonly ILogger<ArchivoCargaController> _logger;
        private readonly ICargarArchivoInputPort _inputPort;
        private readonly ICargarArchivoOutputPort _outputPort;
        public ArchivoCargaController(ICargarArchivoInputPort inputPort, ICargarArchivoOutputPort outputPort, ILogger<ArchivoCargaController> logger)
        {
            _inputPort = inputPort;
            _outputPort = outputPort;
            _logger = logger;
        }

        /// <summary>
        /// Carga un archivo Excel
        /// </summary>
        /// <param name="archivo">Archivo Excel a cargar</param>
        /// <param name="usuario">Usuario que realiza la carga</param>
        /// <returns>Resultado de la carga</returns>
        [HttpPost("CargarArchivo")]
        [Consumes("multipart/form-data")]
        public async Task<JSON<CargaArchivoDTO>> CargarArchivo(IFormFile archivo, [FromForm] string usuario)
        {
            await _inputPort.Handle(new PeticionCargaArchivoDTO { Archivo = archivo, Usuario = usuario });
            return ((IPresenteDataResponse<JSON<CargaArchivoDTO>>)_outputPort).Contenido;
        }




    }
}
