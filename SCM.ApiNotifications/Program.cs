using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SCM.ApiNotifications.Infraestructura;
using SCM.ApiNotifications.Infraestructura.Messaging;
using SCM.ApiNotifications.Infraestructura.Repositorio;
using SCM.ApiNotifications.Framework.Presentador;
using SCM.ApiNotifications.Aplicacion.UseCase;
using SCM.ApiNotifications.Transversal;
using SCM.Shared.EventBus.DependencyInjection;
using System.Text;

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

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Test API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
    //option.OperationFilter<FileUploadOperationFilter>();
});

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Configuration
    .AddJsonFile("Const/MensajesGenerados.json", true, true);

builder.Services.AddUseCaseServicios();
builder.Services.AddPresenters();
builder.Services.AddRepositorio(builder.Configuration);
//builder.Services.AddFileServer();
builder.Services.AddTransversalServicios();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.IncludeErrorDetails = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ClockSkew = TimeSpan.Zero,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
        ),
    };
});


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
