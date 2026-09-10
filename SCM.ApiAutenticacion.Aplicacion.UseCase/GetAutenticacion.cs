using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SCM.ApiAutenticacion.Aplicacion.DTO.Request;
using SCM.ApiAutenticacion.Aplicacion.DTO.Response;
using SCM.ApiAutenticacion.Aplicacion.UseCasePorts.InputPort;
using SCM.ApiAutenticacion.Aplicacion.UseCasePorts.OutputPort;
using SCM.ApiAutenticacion.Dominio.Entidad;
using SCM.ApiAutenticacion.Dominio.Interface;
using SCM.ApiAutenticacion.Transversal.Soporte;
using System.Security.Claims;


namespace SCM.ApiAutenticacion.Aplicacion.UseCase
{
    public class GetAutenticacion : IGetAutenticacionInputPort
    {
        private readonly IUserRepositorio _repositorio;
        private readonly IAutenticacionTokenOutputPort _outputPort;
        //private readonly IInsertarTokenAccesoInputPort _insertarAccesoTokenInputPort;
        private readonly IConfiguration _config;
        private readonly ILogger<GetAutenticacion> _logger;

        public GetAutenticacion(IUserRepositorio repositorio,
                                        IAutenticacionTokenOutputPort outputPort,
                                        //IInsertarTokenAccesoInputPort insertarAccesoTokenInputPort,
                                        IConfiguration config,
                                        ILogger<GetAutenticacion> logger) =>
            (_repositorio, _outputPort, /*_insertarAccesoTokenInputPort,*/ _config, _logger) =
            (repositorio, outputPort,/* insertarAccesoTokenInputPort,*/ config, logger);

        public async Task Handle(GetAutenticacionDTO request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Usuario))
                    throw new AbandonedMutexException($"41");
                if (string.IsNullOrEmpty(request.Clave))
                    throw new AbandonedMutexException($"42");

                User usuario = await _repositorio.Authenticate(request.Usuario, request.Clave);

                if (usuario is null)
                    throw new AbandonedMutexException($"44");

                if (request.Usuario is null)
                    request.Usuario = String.Empty;

                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Sid, usuario.UserId.ToString()),
                        new Claim(ClaimTypes.Name, usuario.LastName + " " + usuario.FirstName)
                    };

                var Token = JwtGenerador.JwtToken(claims, _config["Jwt:Key"]!, _config["Jwt:Issuer"]!, _config["Jwt:Audience"]!, int.Parse(_config["Jwt:TimeSegundosToken"]!));

                //await _insertarAccesoTokenInputPort.Handle(usuario.id,Token, resquest.UsuarioAplicacion);
                if (Token is not null)
                {
                    ResponseHeaderDTO responseHeader = new();
                    TokenDTO data = new();
                    responseHeader.Codigo = "20";
                    responseHeader.Mensaje = _config["MSJ:20"];
                    data.Token = Token;
                    data.Usuario = request.Usuario;

                    await _outputPort.Handle(responseHeader, data);
                }
            }
            catch (Exception ex)
            {
                TokenDTO data = new();
                ResponseHeaderDTO responseHeader = new();
                switch (ex.Message)
                {
                    case "44":
                        responseHeader.Codigo = "44";
                        responseHeader.Mensaje = _config["MSJ:44"];
                        break;
                    case "41":
                        responseHeader.Codigo = "41";
                        responseHeader.Mensaje = _config["MSJ:41"];
                        break; 
                    case "42":
                        responseHeader.Codigo = "42";
                        responseHeader.Mensaje = _config["MSJ:42"];
                        break;                      
                    default:
                        responseHeader.Codigo = "53";
                        responseHeader.Mensaje = _config["MSJ:53"];
                        _logger.LogError("AUTENTICACIONGET-53 GetAutenticacion - Handle \n" + ex.ToString());
                        break;
                }
                await _outputPort.Handle(responseHeader, data);
            }
        }
    }
}
