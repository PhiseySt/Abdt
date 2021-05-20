using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TextService.Repositories
{
    public static class TextDbOptionExtension
    {
        public static void AddTextDbOption(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<TextDbOption>(options =>
            options.ConnectionString = configuration.GetConnectionString("Text"));
        }
    }
}
