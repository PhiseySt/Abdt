using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Refit;
using System;
using System.Net.Http;

namespace FindService.Client.Configuration
{
    public static class FindServiceClientConfiguration
    {
        public static IServiceCollection AddFindServiceClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddTransient(_ => RestService.For<IFindClient>(
                new HttpClient(
                new HttpClientHandler { ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true })
                {
                    BaseAddress = new Uri(configuration["ServiceUrls:FindService"])
                }));
            
            return services;
        }
    }
}
