using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SCM.ApiNotifications.Aplicacion.DTO.Request;
using SCM.ApiNotifications.Aplicacion.UseCasePorts.InputPort;
using SCM.ApiNotifications.Aplicacion.UseCasePorts.OutputPort;
using SCM.ApiNotifications.Dominio.Interface;
using SCM.Shared.EventBus.Abstractions;
using MailKit.Net.Smtp;
using MimeKit;
using SCM.ApiNotifications.Aplicacion.DTO.Response;


namespace SCM.ApiNotifications.Aplicacion.UseCase
{
    public class NotificarUseCase : INotificarInputPort
    {
        private readonly INotificationsRepositorio _notificationsRepositorio;
        private readonly ILogger<NotificarUseCase> _logger;
        private readonly IEventBus _eventBus;
        private readonly IConfiguration _configuration;
        private readonly INotificarOutputPort _outputPort;
        public NotificarUseCase(
                INotificationsRepositorio notificationsRepositorio, 
                ILogger<NotificarUseCase> logger, 
                IEventBus eventBus, 
                IConfiguration configuration, 
                INotificarOutputPort outputPort)
        {
            _logger = logger;
            _notificationsRepositorio = notificationsRepositorio;
            _eventBus = eventBus;
            _configuration = configuration;
            _outputPort = outputPort;
        }

        public async Task Handle(PeticionNotificarDTO request)
        {

            try
            {
                //string smtpServer = Environment.GetEnvironmentVariable("SMTP_SERVER") ?? "localhost";
                //int smtpPort = int.Parse(Environment.GetEnvironmentVariable("SMTP_PORT") ?? "1025");

                var archivoCarga = await _notificationsRepositorio.ObtenerCargaArchivoAsync(request.IdCarga);

                string smtpServer = _configuration["EmailSettings:SmtpServer"] ?? "localhost";
                int smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "1025");
                string smtpUsername = _configuration["EmailSettings:Username"] ?? "";
                string smtpPassword = _configuration["EmailSettings:Password"] ?? "";

                Console.WriteLine($"Conectando al servidor SMTP: {smtpServer}:{smtpPort}...");

                // Crear el mensaje de correo
                var mensaje = new MimeMessage();
                mensaje.From.Add(new MailboxAddress("Mi App .NET", "no-reply@miapp.com"));
                mensaje.To.Add(new MailboxAddress("Destinatario de Prueba", archivoCarga.Usuario));
                mensaje.Subject = "¡Hola desde SCM Notificactions!";

                mensaje.Body = new TextPart("plain")
                {
                    Text = $"Se confirma que que el archivo de carga de Id: {archivoCarga.IdCarga} ha sido procesado satisfactoriamente."
                };

                // Enviar el correo usando SmtpClient de MailKit
                using (var cliente = new SmtpClient())
                {
                    // Para Gmail: usar StartTls en puerto 587
                    // Para Mailpit (desarrollo): usar SecureSocketOptions.None en puerto 1025
                    var secureSocketOptions = smtpServer.Contains("gmail")
                        ? MailKit.Security.SecureSocketOptions.StartTls
                        : MailKit.Security.SecureSocketOptions.None;

                    await cliente.ConnectAsync(smtpServer, smtpPort, secureSocketOptions);

                    // Si el servidor requiere autenticación (como Gmail)
                    if (!string.IsNullOrEmpty(smtpUsername) && !string.IsNullOrEmpty(smtpPassword))
                    {
                        await cliente.AuthenticateAsync(smtpUsername, smtpPassword);
                    }

                    await cliente.SendAsync(mensaje);
                    Console.WriteLine("¡Correo enviado con éxito!");

                    await _notificationsRepositorio.ActualizarEstadoCargaArchivoAsync(archivoCarga.IdCarga, "Notificado");

                    await cliente.DisconnectAsync(true);
                }

                ResponseHeaderDTO responseHeaderDTO = new ();
                ResponseNotificarDTO data = new();
                responseHeaderDTO.Codigo = "200";
                responseHeaderDTO.Mensaje = "Notificación enviada con éxito";
                data.idCarga = request.IdCarga;
                data.usuario = request.Usuario;

                await _outputPort.Handle(responseHeaderDTO,data);

            }
            catch (Exception ex)
            {
                ResponseHeaderDTO responseHeaderDTO = new();
                ResponseNotificarDTO data = new();
                _logger.LogError($"Error al enviar el correo: {ex.Message}");
                await _outputPort.Handle(responseHeaderDTO, data);
            }
        }
    }
}
