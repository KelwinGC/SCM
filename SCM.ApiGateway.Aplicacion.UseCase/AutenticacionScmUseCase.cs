using AutoMapper;
using SCM.ApiGateway.Aplicacion.DTO.Request;
using SCM.ApiGateway.Aplicacion.DTO.Response;
using SCM.ApiGateway.Aplicacion.UseCasePorts.InputPort;
using SCM.ApiGateway.Aplicacion.UseCasePorts.OutputPort;
using SCM.ApiGateway.Infraestructura.WebServices.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SCM.ApiGateway.Aplicacion.DTO.Response.Services;


namespace SCM.ApiGateway.Aplicacion.UseCase
{
    public class AutenticacionScmUseCase : IAutenticacionInputPort
    {
        private readonly IAutenticacionOutputPort _outputPort;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AutenticacionScmUseCase> _logger;
        private readonly IMapper _mapper;
        private readonly IAutenticacionScm _autenticacionScm;

        public AutenticacionScmUseCase(IAutenticacionOutputPort outputPort, 
                                    IConfiguration configuration,
                                    ILogger<AutenticacionScmUseCase> logger,
                                    IMapper mapper,
                                    IAutenticacionScm autenticacionScm)
        {
            _configuration = configuration;
            _outputPort = outputPort;
            _logger = logger;
            _mapper = mapper;
            _autenticacionScm = autenticacionScm;
        }

        public async Task Handle(AccesosRequestDto request)
        {
            ResponseGenericoDto<TokenAutenticacionResponseDto> response = new();
            try
            {
                AutenticacionScmResponse responseApiRestScm =  await _autenticacionScm.Autenticar(request.Usuario!, request.Clave!);
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
