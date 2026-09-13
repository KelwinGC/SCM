using AutoMapper;
using SCM.ApiGateway.Aplicacion.DTO.Request;
using SCM.ApiGateway.Aplicacion.DTO.Response;
using SCM.ApiGateway.Aplicacion.UseCasePorts.InputPort;
using SCM.ApiGateway.Aplicacion.UseCasePorts.OutputPort;
using SCM.ApiGateway.Infraestructura.WebServices.Interface;
using SCM.ApiGateway.Infraestructura.WebServices.Response;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace SCM.ApiGateway.Aplicacion.UseCase
{
    public class AutenticacionScm : IAutenticacionInputPort
    {
        private readonly IAutenticacionOutputPort _outputPort;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AutenticacionScm> _logger;
        private readonly IMapper _mapper;
        private readonly IAutenticacionKeyScm _autenticacionKeyScm;

        public AutenticacionScm(IAutenticacionOutputPort outputPort, 
                                    IConfiguration configuration,
                                    ILogger<AutenticacionScm> logger,
                                    IMapper mapper,
                                    IAutenticacionKeyScm autenticacionKeyScm)
        {
            _configuration = configuration;
            _outputPort = outputPort;
            _logger = logger;
            _mapper = mapper;
            _autenticacionKeyScm = autenticacionKeyScm;
        }

        public async Task Handle(AccesosRequest request)
        {
            ResponseGenericoDto<TokenAutenticacionResponseDto> response = new();
            try
            {
                AutenticacionKeyScmResponse responseApiRestScm =  await _autenticacionKeyScm.AutenticacionScm(request.Usuario!, request.Clave!);
                response = _mapper.Map<ResponseGenericoDto<TokenAutenticacionResponseDto>>(responseApiRestScm);
            }catch (Exception ex)
            {
                response.Codigo = "10004";
                response.Mensaje = _configuration["MCGS:10004"]!.Replace("{NomServicio}", "Autenticacion SCM").Replace("{capa}", "Aplicacion UseCase");
                _logger.LogError($"SCM ApiGateway Aplicacion UseCase Autenticacion AutenticacionScm Handle \n {response.Mensaje}  \n {ex.Message} ");    
            }

            await _outputPort.Handle(response);
        }
    }
}
