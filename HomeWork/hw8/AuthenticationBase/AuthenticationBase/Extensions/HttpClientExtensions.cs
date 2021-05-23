using AuthenticationBase.Handlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Refit;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace AuthenticationBase.Extensions
{
    public static class HttpClientExtensions
    {
        private const string DefaultUrlKey = "Api:Url";

        public static IServiceCollection AddApiClient<T>(this IServiceCollection services,
            IConfiguration configuration,
            RefitSettings refitSettings,
            string configurationUrlKey = null) where T : class
        {
            services.AddHttpClientHandlers();
            services.Configure<ApiSettings>(configuration.GetSection("Api"));

            var baseUrl = configuration[configurationUrlKey ?? DefaultUrlKey];

            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new NullReferenceException($"BaseUrl is empty for {nameof(T)}");

            services.TryAddTransient<T>(provider =>
            {
                // HACK клиент собирается руками, т.к. factory не учитывает зарегистрированное время жизни для HttpMessageHandler
                // https://github.com/dotnet/runtime/issues/36574

                var httpMessageHandler = provider.BuildHandler();
                var client = new HttpClient(httpMessageHandler)
                {
                    BaseAddress = new Uri(baseUrl.TrimEnd('/'))
                };

                return RestService.For<T>(client, refitSettings);
            });

            return services;
        }

        public static IServiceCollection AddStaticBearerTokenApiClient<T>(this IServiceCollection services,
    IConfiguration configuration,
    RefitSettings refitSettings, string bearerToken = default,
    string configurationUrlKey = null) where T : class
        {
            services.AddHttpClientHandlers();
            services.Configure<ApiSettings>(configuration.GetSection("Api"));

            var baseUrl = configuration[configurationUrlKey ?? DefaultUrlKey];

            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new NullReferenceException($"BaseUrl is empty for {nameof(T)}");

            services.TryAddTransient<T>(provider =>
            {
                // HACK клиент собирается руками, т.к. factory не учитывает зарегистрированное время жизни для HttpMessageHandler
                // https://github.com/dotnet/runtime/issues/36574

                var httpMessageHandler = provider.BuildHandler();
                var client = new HttpClient(httpMessageHandler)
                {
                    BaseAddress = new Uri(baseUrl.TrimEnd('/'))
                };
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

                return RestService.For<T>(client, refitSettings);
            });

            return services;
        }

        internal static IServiceCollection AddHttpClientHandlers(this IServiceCollection services)
        {
            services.TryAddTransient<AuthenticatedHttpClientHandler>();

            return services;
        }

        private static HttpMessageHandler BuildHandler(this IServiceProvider provider)
        {
            var authHandler = provider.GetService<AuthenticatedHttpClientHandler>();

            return authHandler;
        }
    }

}
