using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Refit;
using System;
using System.Net.Http;

namespace TextService.Client.Configuration
{
    public static class TextServiceClientConfiguration
    {
        public static IServiceCollection AddTextServiceClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddTransient(_ => RestService.For<ITextClient>(
                new HttpClient
                (
                    new HttpClientHandler { ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true }
                )
                {
                    BaseAddress = new Uri(configuration["ServiceUrls:TextService"]),
                    Timeout = TimeSpan.FromMinutes(5)
                }));

            return services;
        }
    }
}
