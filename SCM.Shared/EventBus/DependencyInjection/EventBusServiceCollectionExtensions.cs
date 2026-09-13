using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SCM.Shared.EventBus.Abstractions;

namespace SCM.Shared.EventBus.DependencyInjection;

public static class EventBusServiceCollectionExtensions
{
    /// <summary>
    /// Registra IEventBus + MassTransit con RabbitMQ. Cada microservicio decide,
    /// con los dos delegados, que consumers registrar y como bindear sus colas
    /// (o ninguno, si el servicio solo publica).
    ///
    /// Notar que NO se usa "services.BuildServiceProvider()" en ningun lado:
    /// las opciones de RabbitMq se leen directo de IConfiguration antes de
    /// llamar a AddMassTransit, evitando el anti-patron de construir un
    /// segundo contenedor de DI dentro de la configuracion del bus.
    /// </summary>
    public static IServiceCollection AddRabbitMqEventBus(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<IBusRegistrationConfigurator> configureConsumers,
        Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator> configureEndpoints)
    {
        var options = configuration.GetSection("RabbitMq").Get<RabbitMqOptions>() ?? new RabbitMqOptions();

        services.AddScoped<IEventBus, EventBusMassTransit>();

        services.AddMassTransit(busConfigurator =>
        {
            configureConsumers(busConfigurator);

            busConfigurator.UsingRabbitMq((context, rabbitCfg) =>
            {
                rabbitCfg.Host(options.HostName, options.VirtualHost, host =>
                {
                    host.Username(options.UserName);
                    host.Password(options.Password);
                });

                // 3 reintentos con 5s de espera antes de mover el mensaje a la
                // cola de error (<nombre-cola>_error) que MassTransit crea sola.
                rabbitCfg.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));

                configureEndpoints(context, rabbitCfg);
            });
        });

        return services;
    }
}
