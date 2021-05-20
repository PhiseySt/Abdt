using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using TextService.Repositories;
using TextService.Repositories.Contexts;
using TextService.Repositories.Interfaces;
using TextService.Repositories.Repositories;
using TextService.Services.Interfaces;
using TextService.Services.TextDapperService;

namespace TextService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TextService", Version = "v1" });
            });

            services.AddTextDbOption(Configuration);
            services.AddAutoMapper(typeof(Startup));
            services.AddTransient(typeof(TextContext));

            services.AddTransient<ITextDapperRepository, TextDapperRepository>();
            services.AddTransient<ITextService, TextDapperService>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TextService v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
