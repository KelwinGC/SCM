using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SCM.ApiGateway.Aplicacion.DTO.Response.Services;
using SCM.ApiGateway.Infraestructura.WebServices.Interface;
using System.Text;
using System.Text.Json;

namespace SCM.ApiGateway.Infraestructura.WebServices.Servicios
{
    public class AutenticacionScm : IAutenticacionScm
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AutenticacionScm> _logger;

        public AutenticacionScm(IHttpClientFactory httpClientFactory,
                                                    IConfiguration configuration,
                                                    ILogger<AutenticacionScm> logger) =>
                                                    (_httpClientFactory, _configuration, _logger) =
                                                    (httpClientFactory, configuration, logger);

        public async Task<AutenticacionScmResponse> Autenticar(string usuario, string clave)
        {
            AutenticacionScmResponse responseApiRest = new();
            try
            {
                #region Consolidar Request
                Dictionary<string, string> request = new()
                {
                    { "usuario", usuario },
                    { "clave", clave }
                };
                string json = JsonSerializer.Serialize(request);
                using StringContent jsonContent = new(json, Encoding.UTF8, "application/json");
                #endregion

                string uri = _configuration["SCM:AUTENTICACION:Uri"]!.Replace(_configuration["SCM:AUTENTICACION:Server"]!, "");

                string? httpClientName = _configuration["SCM:AUTENTICACION:Name"];
                using HttpClient _httpClain = _httpClientFactory.CreateClient(httpClientName ?? "");
                using var response = await _httpClain.PostAsync(uri, jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    var dato = await response.Content.ReadAsStringAsync();                   
                    responseApiRest = JsonSerializer.Deserialize<AutenticacionScmResponse>(dato, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
                }
                else
                {
                    responseApiRest.Codigo = "150";
                    responseApiRest.Mensaje = _configuration["MCGS:150"]!.Replace("{NomServicio}", "Autenticación SCM").Replace("{Codigo}", $"{(int)response.StatusCode}");

                }
            }
            catch (Exception ex)
            {
                responseApiRest.Codigo = "151";
                responseApiRest.Mensaje = _configuration["MCGS:151"]!.Replace("{NomServicio}", "Autenticación SCM");
                _logger.LogError($"SCM ApiGateway Infraestructura WebServices Servicios AutenticacionKeyScm AutenticacionScm \n {responseApiRest.Mensaje} \n {ex.Message} ");
            }
            return responseApiRest;
        }
    }
}
