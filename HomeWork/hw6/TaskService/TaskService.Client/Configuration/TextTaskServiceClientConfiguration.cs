using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Refit;
using System;
using System.Net.Http;

namespace TaskService.Client.Configuration
{
    public static class TextTaskServiceClientConfiguration
    {
        public static IServiceCollection AddTaskServiceClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddTransient(_ => RestService.For<ITaskClient>(new HttpClient(
                new HttpClientHandler{ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true})
                {
                    BaseAddress = new Uri(configuration["ServiceUrls:TaskService"])
                }));

            return services;
        }


    }
}
