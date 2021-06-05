using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Refit;
using System;
using System.Net.Http;


namespace Openweathermap.Client.Configuration
{
    public static class OpenweathermapClientConfiguration
    {
        public static IServiceCollection AddOpenweathermapClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddTransient(_ => RestService.For<IOpenweathermapClient>(new HttpClient(
                new HttpClientHandler { ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true })
            {
                BaseAddress = new Uri(configuration["ServiceUrls:OpenweathermapService"])
            }));

            return services;
        }
     }
}
