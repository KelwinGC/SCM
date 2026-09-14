using System.Net.Http.Headers;

namespace SCM.ApiGateway.ScmServices
{
    public static class Services
    {
        public static IServiceCollection AddServicesScm(this IServiceCollection services, IConfiguration configuration)
        {
            string? httpClientAut = configuration["SCM:AUTENTICACION:Name"];
            ArgumentException.ThrowIfNullOrEmpty(httpClientAut);
            services.AddHttpClient(
                httpClientAut,
                client =>
                {
                    client.BaseAddress = new Uri(configuration["SCM:AUTENTICACION:Server"]!);
                });

            string? httpClientControl = configuration["SCM:CONTROL:Name"];
            ArgumentException.ThrowIfNullOrEmpty(httpClientControl);
            services.AddHttpClient(
                httpClientControl,
                client =>
                {
                    // Set the base address of the named client.
                    client.BaseAddress = new Uri(configuration["SCM:CONTROL:Server"]!);
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("");
                    // Add a user-agent default request header.
                    //client.DefaultRequestHeaders.UserAgent.ParseAdd("dotnet-docs");
                });


            return services;    

        }
    }
}
