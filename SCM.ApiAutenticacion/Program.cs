using SCM.ApiAutenticacion.Aplicacion.UseCase;
using SCM.ApiAutenticacion.Framework.Presentador;
using SCM.ApiAutenticacion.Infraestructura.Repositorio;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Logging.AddLog4Net();

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRepositorio(builder.Configuration);
builder.Services.AddUseCaseServicios();
builder.Services.AddPresenters();
builder.Configuration
    .AddJsonFile("envglobal.json", true, true);

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
