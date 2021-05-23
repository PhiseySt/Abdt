using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TaskService.Repositories
{
    public static class TaskDbOptionExtension
    {
        public static void AddSqlTaskDbOption(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<TaskDbOption>(options =>
                options.ConnectionString = configuration.GetConnectionString("SqlConnection"));
        }

        public static void AddPostgreTaskDbOption(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<TaskDbOption>(options =>
                options.ConnectionString = configuration.GetConnectionString("PostgreConnection"));
        }
    }
}