using AuthenticationBase.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Refit;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AuthenticationService.Client.Configuration
{
    public static class AuthenticationClientConfiguration
    {
        public static IServiceCollection AddAuthenticationServiceClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddTransient(_ => RestService.For<IAuthenticationClient>(
                new HttpClient
                (
                    new HttpClientHandler { ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true }
                )
                {
                    BaseAddress = new Uri(configuration["ServiceUrls:AuthenticationService"]),
                }));

            return services;
        }
        public static IServiceCollection AddAuthenticationServiceGetTokenClient(this IServiceCollection services, IConfiguration configuration)
        {
            var refitSettings = new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(configuration["Token"])
                //AuthorizationHeaderValueGetter = () => new AuthService().GetToken(new LoginModel { Username = "QWE", Password = "QweAsd123!" })
            };

            services.AddApiClient<IAuthenticationClient>(configuration, refitSettings, "ServiceUrls:AuthenticationService");

            return services;
        }
    }
}
