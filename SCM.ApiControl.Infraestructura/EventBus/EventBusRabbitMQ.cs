using MassTransit;
using Microsoft.Extensions.Logging;
using SCM.ApiControl.Dominio.Events;
using SCM.ApiControl.Dominio.Interface;

namespace SCM.ApiControl.Infraestructura.EventBus
{
    public class EventBusRabbitMQ : IEventBusOld
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<EventBusRabbitMQ> _logger;

        public EventBusRabbitMQ(IPublishEndpoint publishEndpoint, ILogger<EventBusRabbitMQ> logger)
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }   


        //public async Task PublishAsync<T>(T @event)
        //{
        //    await _publishEndpoint.Publish(@event);
        //    _logger.LogInformation(
        //  "Evento publicado mediante MassTransit. EventType={EventType}",@event.GetType()
        //  );
        //}

        public async Task PublishAsync(
        IntegrationEvent @event,
        CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(@event);

            await _publishEndpoint.Publish(@event, @event.GetType(), cancellationToken);

            _logger.LogInformation(
                "Evento publicado mediante MassTransit. EventType={EventType}, EventId={EventId}",
                @event.GetType().Name,
                @event.Id);
        }

        // Mantener para compatibilidad con código existente
        //public void Publish<T>(T @event)
        //{
        //    PublishAsync(@event).GetAwaiter().GetResult();
        //}
    }
}
