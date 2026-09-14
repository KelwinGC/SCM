
using SCM.ApiLoadProcess.Dominio.Events;

namespace SCM.ApiLoadProcess.Dominio.Interface
{
    public interface IEventBusOld
    {
        //void Publish<T>(T @event);
        Task PublishAsync<T>(T @event);

        //Task PublishAsync(IntegrationEvent @event, CancellationToken cancellationToken = default);

    }
}
