using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SCM.ApiGateway.Aplicacion.DTO.Request;
using SCM.ApiGateway.Aplicacion.DTO.Response;
using SCM.ApiGateway.Aplicacion.DTO.Response.Services;
using SCM.ApiGateway.Aplicacion.UseCasePorts.InputPort;
using SCM.ApiGateway.Aplicacion.UseCasePorts.OutputPort;
using SCM.ApiGateway.Infraestructura.WebServices.Interface;

namespace SCM.ApiGateway.Aplicacion.UseCase
{
    public class ControlScmUseCase : IControlInputPort
    {
        private readonly IControlOutputPort _outputPort;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ControlScmUseCase> _logger;
        private readonly IMapper _mapper;
        private readonly IControlScm _controlScm;

        public ControlScmUseCase(
            IControlOutputPort outputPort,
            IConfiguration configuration, 
            ILogger<ControlScmUseCase> logger, 
            IMapper mapper,
            IControlScm controlScm
            )
        {
            _outputPort = outputPort;
            _configuration = configuration;
            _logger = logger;
            _mapper = mapper;
            _controlScm = controlScm;   
        }

        public async Task Handle(ControlRequestDto request)
        {
            ResponseGenericoDto<ControlResponseDto> response = new();
            try
            {
                ControlScmResponse responseApiRest = await _controlScm.CargarArchivo(request);
                response = _mapper.Map<ResponseGenericoDto<ControlResponseDto>>(responseApiRest);
            }
            catch (Exception ex)
            {
                response.Codigo = "104";
                response.Mensaje = _configuration["MCGS:104"]!.Replace("{NomServicio}", "Control SCM").Replace("{capa}", "Aplicacion UseCase");
                _logger.LogError($"SCM ApiGateway Aplicacion UseCase Control ControlScm Handle \n {response.Mensaje}  \n {ex.Message} ");
            }

            await _outputPort.Handle(response);

            //return Task.CompletedTask;
        }
    }
}
