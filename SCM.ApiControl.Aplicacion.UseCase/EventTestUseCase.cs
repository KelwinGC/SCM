using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SCM.ApiControl.Aplicacion.DTO.Request;
using SCM.ApiControl.Aplicacion.DTO.Response;
using SCM.ApiControl.Aplicacion.UseCasePorts.InputPort;
using SCM.ApiControl.Aplicacion.UseCasePorts.OutputPort;
//using SCM.ApiControl.Dominio.Events;
//using SCM.ApiControl.Dominio.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using SCM.Shared.EventBus.Abstractions;
using SCM.Shared.Contracts;

namespace SCM.ApiControl.Aplicacion.UseCase
{
    public class EventTestUseCase : IEventTestInputPort
    {
        private readonly IEventTestOutputPort _outputPort;
        private readonly IEventBus _eventBus;
        private readonly IConfiguration _configuration;
        private readonly ILogger<EventTestUseCase> _logger;

        public EventTestUseCase(IEventBus eventBus, IConfiguration configuration, ILogger<EventTestUseCase> logger, IEventTestOutputPort outputPort)
        {
            _eventBus = eventBus;
            _configuration = configuration;
            _logger = logger;
            _outputPort = outputPort;
        }

        public async Task Handle(PeticionEventTestDTO request)
        {
            try
            {
                //var @event = new CargaArchivoCreatedIntegrationEvent(request.IdCarga, request.RutaArchivo, request.Usuario);
                var @event = new CargaArchivoCreatedEvent(request.IdCarga, request.RutaArchivo, request.Usuario);
                await _eventBus.PublishAsync(@event);

                ResponseHeaderDTO responseHeaderDTO = new ResponseHeaderDTO
                {
                    Codigo = "200",
                    Mensaje = "Evento publicado correctamente"
                };
                ResponseEventTestDTO data = new ResponseEventTestDTO
                {
                    Id = @event.Id.ToString(),
                    IdCarga = request.IdCarga,
                    RutaArchivo = request.RutaArchivo,
                    Usuario = request.Usuario,
                    CreationDate = @event.CreationDate,
                };
                await _outputPort.Handle(responseHeaderDTO, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al manejar el evento de prueba");
                //throw;
                ResponseHeaderDTO responseHeaderDTO = new();
                ResponseEventTestDTO data = new();

                responseHeaderDTO.Codigo = "500";
                responseHeaderDTO.Mensaje = "Error al manejar el evento de prueba: " + ex.Message;

                await _outputPort.Handle(responseHeaderDTO, data);

            }

        }
    }
}
