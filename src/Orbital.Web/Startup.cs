using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Converters;
using Orbital.Data.Connections;
using Orbital.Data.Repositories;
using Orbital.Models.Repositories;
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

            DatabaseMigrator.Migrate(ConnectionString);
        }

        public IConfigurationRoot Configuration { get; }
        public string ConnectionString => Configuration["connectionString"];

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDatabase(ConnectionString);
            services.AddRepositories();
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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
        }
    }

    public static class ServiceExtensions
    {
        public static void AddDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton<IDbConnectionFactory>(new PostgresqlConnectionFactory(connectionString));
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBadgeHolderRepository, DatabaseBadgeHolderRepository>();
            services.AddScoped<IBadgeRepository, DatabaseBadgeRepository>();
            services.AddScoped<IClubRepository, DatabaseClubRepository>();
            services.AddScoped<ICompetitionRepository, DatabaseCompetitionRepository>();
            services.AddScoped<IHandicapRepository, DatabaseHandicapRepository>();
            services.AddScoped<IPersonRepository, DatabasePersonRepository>();
            services.AddScoped<IRecordRepository, DatabaseRecordRepository>();
            // TODO services.AddScoped<IRecordTeamRepository, DatabaseRecordTeamRepository>();
            services.AddScoped<IRoundRepository, DatabaseRoundRepository>();
            services.AddScoped<IScoreRepository, DatabaseScoreRepository>();
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IBadgeService, BadgeService>();
            services.AddScoped<IClubService, ClubService>();
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IRoundService, RoundService>();
        }
    }
}
