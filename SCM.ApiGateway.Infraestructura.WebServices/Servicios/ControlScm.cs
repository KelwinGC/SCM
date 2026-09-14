using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SCM.ApiGateway.Aplicacion.DTO.Request;
using SCM.ApiGateway.Aplicacion.DTO.Response.Services;
using SCM.ApiGateway.Infraestructura.WebServices.Interface;
using SCM.ApiGateway.Transversal.Interface;
using System.Text.Json;


namespace SCM.ApiGateway.Infraestructura.WebServices.Servicios
{
    public class ControlScm: IControlScm
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ControlScm> _logger;
        private readonly ISecurity _security;

        public ControlScm(IHttpClientFactory httpClientFactory,
                          IConfiguration configuration,
                          ILogger<ControlScm> logger,
                          ISecurity security) =>
                          (_httpClientFactory, _configuration, _logger, _security) =
                          (httpClientFactory, configuration, logger, security);
        public async Task<ControlScmResponse> CargarArchivo(ControlRequestDto requestDto)
        {
            ControlScmResponse responseApiRest = new();
            try
            {
                #region Consolidar Request
                using var content = new MultipartFormDataContent();

                // Agregar el archivo
                using var fileStream = requestDto.Archivo.OpenReadStream();
                content.Add(new StreamContent(fileStream), "archivo", requestDto.Archivo.FileName);

                // Agregar el usuario como campo de texto
                content.Add(new StringContent(requestDto.Usuario), "usuario");
                #endregion

                string uri = _configuration["SCM:CONTROL:Uri"]!.Replace(_configuration["SCM:CONTROL:Server"]!, "");

                string? httpClientName = _configuration["SCM:CONTROL:Name"];
                using HttpClient _httpClain = _httpClientFactory.CreateClient(httpClientName ?? "");

                // Obtener el token del usuario actual
                var userInfo = _security.GetUser();
                var token = userInfo["Token"];

                // Agregar el token al header de autorización
                _httpClain.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                using var response = await _httpClain.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    var dato = await response.Content.ReadAsStringAsync();
                    responseApiRest = JsonSerializer.Deserialize<ControlScmResponse>(dato, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
                }
                else
                {
                    responseApiRest.Codigo = "150";
                    responseApiRest.Mensaje = _configuration["MCGS:150"]!.Replace("{NomServicio}", "Control SCM").Replace("{Codigo}", $"{(int)response.StatusCode}");

                }
            }
            catch (Exception ex)
            {
                responseApiRest.Codigo = "151";
                responseApiRest.Mensaje = _configuration["MCGS:151"]!.Replace("{NomServicio}", "Control SCM");
                _logger.LogError($"SCM ApiGateway Infraestructura WebServices Servicios ControlScm \n {responseApiRest.Mensaje} \n {ex.Message} ");
            }
            return responseApiRest;
        }
    }
}
