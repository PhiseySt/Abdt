using AuthenticationBase.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Refit;
using System;
using System.Net.Http;
using System.Threading.Tasks;

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

         public static IServiceCollection AddTaskServiceBearerTokenClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApiClient<ITaskClient>(configuration, new RefitSettings(), "ServiceUrls:TaskService");

            return services;
        }

        //Получение токена из appsettings
        public static IServiceCollection AddTaskServiceBearerGetTokenClient(this IServiceCollection services, IConfiguration configuration)
        {
            var refitSettings = new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(configuration["Token"])
            };
            services.AddApiClient<ITaskClient>(configuration, refitSettings, "ServiceUrls:TaskService");

            return services;
        }


    }
}
