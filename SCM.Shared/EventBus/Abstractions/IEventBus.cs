using SCM.Shared.Contracts;

namespace  SCM.Shared.EventBus.Abstractions;

/// <summary>
/// Unica dependencia que controllers y consumers deberian conocer.
/// Ninguno de los 3 microservicios referencia MassTransit directamente
/// fuera de esta libreria (salvo IConsumer&lt;T&gt;, que es inevitable
/// porque MassTransit necesita que los consumers implementen su interfaz
/// para poder invocarlos).
/// </summary>
public interface IEventBus
{
    Task PublishAsync<TIntegrationEvent>(TIntegrationEvent @event, CancellationToken cancellationToken = default)
        where TIntegrationEvent : IntegrationEvent;
}
