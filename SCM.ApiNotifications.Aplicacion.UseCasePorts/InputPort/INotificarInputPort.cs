using SCM.ApiNotifications.Aplicacion.DTO.Request;

namespace SCM.ApiNotifications.Aplicacion.UseCasePorts.InputPort
{
    public interface INotificarInputPort
    {
        Task Handle(PeticionNotificarDTO resquest);

    }
}
