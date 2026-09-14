using SCM.ApiNotifications.Aplicacion.DTO.Response;


namespace SCM.ApiNotifications.Aplicacion.UseCasePorts.OutputPort
{
    public interface INotificarOutputPort
    {
        Task Handle(ResponseHeaderDTO requestHeader, ResponseNotificarDTO responseNotificar);

    }
}
