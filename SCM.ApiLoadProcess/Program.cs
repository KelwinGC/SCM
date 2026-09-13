using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SCM.ApiLoadProcess.Aplicacion.UseCase;
using SCM.ApiLoadProcess.Framework.Presentador;
using SCM.ApiLoadProcess.Infraestructura;
using SCM.ApiLoadProcess.Infraestructura.Repositorio;
using SCM.ApiLoadProcess.Infraestructura.FileServer;
using SCM.ApiLoadProcess.Transversal;
using System.Text;
using SCM.Shared.EventBus.DependencyInjection;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRabbitMqEventBus(
    builder.Configuration,
    configureConsumers: x =>
    {
        x.AddConsumer<CargaArchivoCreatedEventConsumer>();
    },
    configureEndpoints: (context, cfg) =>
    {
        cfg.ReceiveEndpoint("carga_masiva", e =>
        {
            e.ConfigureConsumer<CargaArchivoCreatedEventConsumer>(context);
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
builder.Services.AddFileServer();
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
