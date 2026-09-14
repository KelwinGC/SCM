using SCM.ApiNotifications.Aplicacion.DTO.Response;
using SCM.ApiNotifications.Aplicacion.UseCasePorts.OutputPort;

namespace SCM.ApiNotifications.Framework.Presentador
{
    internal class NotificationJson : INotificarOutputPort, IPresenteDataResponse<JSON<ResponseNotificarDTO>>
    {
        public JSON<ResponseNotificarDTO> Contenido { get; private set; }

        public Task Handle(ResponseHeaderDTO requestHeader, ResponseNotificarDTO responseNotificarDTO)
        {
            Contenido = new JSON<ResponseNotificarDTO>
            {
                Codigo = requestHeader.Codigo,
                Mensaje = requestHeader.Mensaje,
                Data = responseNotificarDTO
            };

            return Task.CompletedTask;
        }
    }
}
