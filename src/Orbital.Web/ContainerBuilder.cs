using Orbital.Data.Connections;
using Orbital.Data.Repositories;
using Orbital.Models.Repositories;
using Orbital.Schema.Clubs;
using Orbital.Schema.People;
using SimpleInjector;

namespace Orbital.Web
{
    class ContainerBuilder
    {
        public static Container Build(string connectionString)
        {
            var container = new Container();

            RegisterDatabase(container, connectionString);
            RegisterRepositories(container);
            RegisterGraphQLServices(container);
            
            container.Verify();
            return container;
        }

        private static void RegisterDatabase(Container container, string connectionString)
        {
            container.RegisterSingleton<IDbConnectionFactory>(new PostgresqlConnectionFactory(connectionString));
        }
        private static void RegisterRepositories(Container container)
        {
            container.Register<IClubRepository, DatabaseClubRepository>();
            container.Register<IPersonRepository, DatabasePersonRepository>();
        }
        private static void RegisterGraphQLServices(Container container)
        {
            container.Register<IClubService, ClubServiceImpl>(Lifestyle.Transient);
            container.Register<IPersonService, PersonServiceImpl>(Lifestyle.Transient);
        }
    }
}
