using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SCM.ApiControl.Dominio.Interface;
using SCM.ApiControl.Infraestructura.EventBus;
using SCM.ApiControl.Infraestructura.EventBus.Options;

namespace SCM.ApiControl.Infraestructura
{
    public static class DependencyConteiner
    {
        public static IServiceCollection AddEventsService(this IServiceCollection services)
        {
            //services.ConfigureOptions<RabbitMqOptionsSetup>();
            //services.AddOptions<RabbitMqSettings>()
            //.Bind(configuration.GetSection(RabbitMqSettings.SectionName))
            //.ValidateDataAnnotations()
            //.ValidateOnStart();

            services.AddScoped<IEventBusOld, EventBusRabbitMQ>();
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    //RabbitMqOptions? opt = services.BuildServiceProvider()
                    //    .GetRequiredService<IOptions<RabbitMqOptions>>()
                    //    .Value;
                    //cfg.Host(opt.HostName, opt.VirtualHost, h =>
                    //{
                    //    h.Username(opt.UserName);
                    //    h.Password(opt.Password);
                    //});

                    var settings = context.GetRequiredService<IOptions<RabbitMqSettings>>().Value;

                    cfg.Host(settings.HostName, settings.Port, settings.VirtualHost, h =>
                    {
                        h.Username(settings.UserName);
                        h.Password(settings.Password);
                    });


                    // Configurar el endpoint para la cola carga_masiva
                    //cfg.Message<CargaArchivoCreatedEvent>(e =>
                    //{
                    //    e.SetEntityName("carga_masiva");
                    //});

                    //cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}

