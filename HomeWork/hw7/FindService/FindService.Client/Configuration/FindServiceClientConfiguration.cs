using AuthenticationBase.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Refit;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FindService.Client.Configuration
{
    public static class FindServiceClientConfiguration
    {
        public static IServiceCollection AddFindServiceClient(this IServiceCollection services, IConfiguration configuration)
        {
            var httpClient = new HttpClient(
                    new HttpClientHandler { ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true })
            {
                BaseAddress = new Uri(configuration["ServiceUrls:FindService"])
            };
            httpClient.DefaultRequestHeaders.Authorization =  new AuthenticationHeaderValue("Bearer",configuration.GetSection("Token").Get<string>());
            services.TryAddTransient(_ => RestService.For<IFindClient>(httpClient));
            
            return services;
        }

  
        public static IServiceCollection AddFindServiceBearerTokenClient(this IServiceCollection services, IConfiguration configuration)
        {
            var refitSettings = new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(configuration["Token"])
            };
            services.AddApiClient<IFindClient>(configuration, refitSettings, "ServiceUrls:FindService");

            return services;
        }

        public static IServiceCollection AddFindServiceStaticBearerTokenClient(this IServiceCollection services, IConfiguration configuration)
        {
            var bearerToken = configuration["Token"];
            services.AddStaticBearerTokenApiClient<IFindClient>(configuration, new RefitSettings(), bearerToken, "ServiceUrls:FindService");

            return services;
        }
    }
}
