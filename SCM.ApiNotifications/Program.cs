using MassTransit;
using SCM.ApiNotifications.Infraestructura;
using SCM.ApiNotifications.Infraestructura.Messaging;
using SCM.Shared.EventBus.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<NotificacionesStore>();

builder.Services.AddRabbitMqEventBus(
    builder.Configuration,
    configureConsumers: x =>
    {
        x.AddConsumer<ArchivoProcesadoEventConsumer>();
    },
    configureEndpoints: (context, cfg) =>
    {
        cfg.ReceiveEndpoint("notificaciones", e =>
        {
            e.ConfigureConsumer<ArchivoProcesadoEventConsumer>(context);
        });
    });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
