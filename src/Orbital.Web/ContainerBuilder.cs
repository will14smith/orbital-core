using Orbital.Data.Connections;
using Orbital.Data.Repositories;
using Orbital.Models.Repositories;
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
            container.Register<IRoundRepository, DatabaseRoundRepository>();
        }


    }
}
