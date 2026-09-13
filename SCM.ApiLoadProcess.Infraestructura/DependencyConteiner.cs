using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using Microsoft.Extensions.Options;
using SCM.ApiLoadProcess.Infraestructura.EventBus.Options;
using SCM.ApiLoadProcess.Dominio.Interface;
using SCM.ApiLoadProcess.Infraestructura.EventBus;
using SCM.ApiLoadProcess.Dominio.Events;

namespace SCM.ApiLoadProcess.Infraestructura
{
    public static class DependencyConteiner
    {
        public static IServiceCollection AddEventsService(this IServiceCollection services)
        {
            services.ConfigureOptions<RabbitMqOptionsSetup>();

            services.AddMassTransit(x =>
            {
                // Registrar el consumer
                x.AddConsumer<CargaArchivoCreatedEventConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    RabbitMqOptions? opt = services.BuildServiceProvider()
                        .GetRequiredService<IOptions<RabbitMqOptions>>()
                        .Value;

                    cfg.Host(opt.HostName, opt.VirtualHost, h =>
                    {
                        h.Username(opt.UserName);
                        h.Password(opt.Password);
                    });

                    // IMPORTANTE: Configurar el endpoint receptor
                    cfg.ReceiveEndpoint("carga_masiva", e =>
                    {
                        // Configurar retry policy
                        e.UseMessageRetry(r =>
                        {
                            r.Interval(3, TimeSpan.FromSeconds(5)); // 3 reintentos con 5 segundos entre intentos
                        });

                        // Configurar concurrencia (cuántos mensajes procesar simultáneamente)
                        e.PrefetchCount = 10;

                        e.ConfigureConsumer<CargaArchivoCreatedEventConsumer>(context);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}

