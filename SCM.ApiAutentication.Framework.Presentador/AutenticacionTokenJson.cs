using SCM.ApiAutenticacion.Aplicacion.DTO.Response;
using SCM.ApiAutenticacion.Aplicacion.UseCasePorts.OutputPort;

namespace SCM.ApiAutenticacion.Framework.Presentador
{
    internal class AutenticacionTokenJson : IAutenticacionTokenOutputPort, IPresenteDataResponse<JSON<TokenDTO>>
    {
        public JSON<TokenDTO> Contenido { get; private set; }

        public Task Handle(ResponseHeaderDTO requestHeader, TokenDTO Token)
        {
            Contenido = new JSON<TokenDTO>
            {
                Codigo = requestHeader.Codigo,
                Mensaje = requestHeader.Mensaje,
                Data = Token
            };

            return Task.CompletedTask;
        }
    }
}
