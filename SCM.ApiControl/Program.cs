using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SCM.ApiControl;
using SCM.ApiControl.Aplicacion.UseCase;
using SCM.ApiControl.Framework.Presentador;
using SCM.ApiControl.Infraestructura.FileServer;
using SCM.ApiControl.Infraestructura.Repositorio;
using SCM.ApiControl.Transversal;
using System.Text;
using SCM.Shared.EventBus.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddOptions<RabbitMqSettings>()
//    .Bind(builder.Configuration.GetSection(RabbitMqSettings.SectionName))
//    .ValidateDataAnnotations()
//    .ValidateOnStart();

builder.Services.AddRabbitMqEventBus(
    builder.Configuration,
    configureConsumers: _ =>
    {
        // Sin consumers: este microservicio nunca lee de ninguna cola,
        // solo publica eventos.
    },
    configureEndpoints: (_, _) =>
    {
        // Sin endpoints que bindear, por la misma razon.
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
    option.OperationFilter<FileUploadOperationFilter>();
});

//builder.Logging.AddLog4Net();
// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Configuration
    //.AddJsonFile("Const/ServiciosExternos.json", true, true)
    .AddJsonFile("Const/MensajesGenerados.json", true, true);

builder.Services.AddUseCaseServicios();
builder.Services.AddPresenters();
//builder.Services.AddWebServices();
//builder.Services.AddEventsService();
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
//app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
