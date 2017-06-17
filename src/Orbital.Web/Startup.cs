using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Converters;
using Orbital.Data;
using Orbital.Web.BadgeHolders;
using Orbital.Web.Badges;
using Orbital.Web.Clubs;
using Orbital.Web.People;
using Orbital.Web.Rounds;
using Swashbuckle.AspNetCore.Swagger;

namespace Orbital.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }
        public string ConnectionString => Configuration["connectionString"];

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOrbitalData(ConnectionString);
            services.AddServices();

            services.AddMvc()
                .AddJsonOptions(opt =>
                {
                    opt.SerializerSettings.Converters.Add(new StringEnumConverter
                    {
                        CamelCaseText = true
                    });
                });
            services.AddLogging();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Orbital", Version = "v1" });
                c.DescribeAllEnumsAsStrings();
                c.DescribeStringEnumsInCamelCase();
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Orbital");
            });

            app.ApplicationServices.MigrateOrbitalData();
        }
    }

    public static class ServiceExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IBadgeService, BadgeService>();
            services.AddScoped<IBadgeHolderService, BadgeHolderService>();
            services.AddScoped<IClubService, ClubService>();
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IRoundService, RoundService>();
        }
    }
}
