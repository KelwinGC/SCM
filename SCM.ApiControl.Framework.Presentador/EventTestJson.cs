using SCM.ApiControl.Aplicacion.DTO.Response;
using SCM.ApiControl.Aplicacion.UseCasePorts.OutputPort;

namespace SCM.ApiControl.Framework.Presentador
{
    internal class EventTestJson : IEventTestOutputPort, IPresenteDataResponse<JSON<ResponseEventTestDTO>>
    {
        public JSON<ResponseEventTestDTO> Contenido { get; private set; }

        public Task Handle(ResponseHeaderDTO requestHeader, ResponseEventTestDTO eventTestDTO)
        {
            Contenido = new JSON<ResponseEventTestDTO>
            {
                Codigo = requestHeader.Codigo,
                Mensaje = requestHeader.Mensaje,
                Data = eventTestDTO
            };

            return Task.CompletedTask;
        }
    }
}
