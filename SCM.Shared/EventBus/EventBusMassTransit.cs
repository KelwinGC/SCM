using MassTransit;
using Microsoft.Extensions.Logging;
using SCM.Shared.Contracts;
using SCM.Shared.EventBus.Abstractions;

namespace SCM.Shared.EventBus;

public class EventBusMassTransit : IEventBus
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<EventBusMassTransit> _logger;

    public EventBusMassTransit(IPublishEndpoint publishEndpoint, ILogger<EventBusMassTransit> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task PublishAsync<TIntegrationEvent>(
        TIntegrationEvent @event,
        CancellationToken cancellationToken = default)
        where TIntegrationEvent : IntegrationEvent
    {
        await _publishEndpoint.Publish(@event, cancellationToken);

        _logger.LogInformation(
            "Evento publicado: {EventName} (Id={EventId})",
            typeof(TIntegrationEvent).Name, @event.Id);
    }
}
