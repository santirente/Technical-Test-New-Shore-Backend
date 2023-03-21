using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newshore.Journeys.Application;
using Newshore.Journeys.Application.ApplicationServices;
using Newshore.Journeys.Domain.Entities;
using Newshore.Journeys.Domain.Repository;
using Newshore.Journeys.Infrastructure.Implementation;
using Newshore.Journeys.Infrastructure.InfrastructureService;

namespace Newshore.Journeys.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddSingleton<IRepository<Journey>>(it =>
            { 
                var liteDbRepository = new LiteDbRepository<Journey>(Configuration);
                return liteDbRepository;
            });

            services.AddSingleton<INewshoreFlightsService>((it) =>
            {
                var newshoreFlightsService = new NewshoreFlightsService(Configuration);
                return newshoreFlightsService;
            });

            services.AddTransient<ICalculateJourneyApplicationService, CalculateJourneyApplicationService>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApplication1", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(option =>
            {
                option.WithOrigins("*");
                option.AllowAnyMethod();
                option.AllowAnyHeader();
            });
            app.ApplicationServices.GetRequiredService<INewshoreFlightsService>().DoInitialCharge();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplication1 v1"));
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
