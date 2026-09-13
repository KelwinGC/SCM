using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SCM.ApiGateway.Infraestructura.WebServices.Interface;
using SCM.ApiGateway.Infraestructura.WebServices.Response;
using System.Text;
using System.Text.Json;

namespace SCM.ApiGateway.Infraestructura.WebServices.Servicios
{
    public class AutenticacionKeyScm : IAutenticacionKeyScm
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AutenticacionKeyScm> _logger;

        public AutenticacionKeyScm(IHttpClientFactory httpClientFactory,
                                                    IConfiguration configuration,
                                                    ILogger<AutenticacionKeyScm> logger) =>
                                                    (_httpClientFactory, _configuration, _logger) =
                                                    (httpClientFactory, configuration, logger);

        public async Task<AutenticacionKeyScmResponse> AutenticacionScm(string usuario, string clave)
        {
            AutenticacionKeyScmResponse responseApiRest = new();
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
                    responseApiRest = JsonSerializer.Deserialize<AutenticacionKeyScmResponse>(dato, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
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
                _logger.LogError($"IRMA ApiGateway Infraestructura WebServices Servicios AutenticacionKeyScm AutenticacionScm \n {responseApiRest.Mensaje} \n {ex.Message} ");
            }
            return responseApiRest;
        }
    }
}
