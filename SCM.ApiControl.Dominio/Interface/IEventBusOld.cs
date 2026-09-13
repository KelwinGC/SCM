
using SCM.ApiControl.Dominio.Events;

namespace SCM.ApiControl.Dominio.Interface
{
    public interface IEventBusOld
    {
        //void Publish<T>(T @event);
        //Task PublishAsync<T>(T @event);

        Task PublishAsync(IntegrationEvent @event, CancellationToken cancellationToken = default);

    }
}
