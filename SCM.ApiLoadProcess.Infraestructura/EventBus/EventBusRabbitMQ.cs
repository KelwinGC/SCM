using MassTransit;
using SCM.ApiLoadProcess.Dominio.Interface;

namespace SCM.ApiLoadProcess.Infraestructura.EventBus
{
    public class EventBusRabbitMQ : IEventBusOld
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public EventBusRabbitMQ(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishAsync<T>(T @event)
        {
            await _publishEndpoint.Publish(@event);
        }

        // Mantener para compatibilidad con código existente
        public void Publish<T>(T @event)
        {
            PublishAsync(@event).GetAwaiter().GetResult();
        }
    }
}
