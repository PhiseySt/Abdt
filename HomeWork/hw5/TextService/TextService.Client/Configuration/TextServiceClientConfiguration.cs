using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Refit;

namespace TextService.Client.Configuration
{
    public static class TextServiceClientConfiguration
    {
        public static IServiceCollection AddTextServiceClient(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.TryAddTransient(_=> RestService.For<ITextClient>(new HttpClient()
            {
                BaseAddress = new Uri(configuration["ServiceUrls:TextService"])
            }));

            return services;
        }
    }
}