using System;
using System.IO;
using GraphQL;
using GraphQL.Http;
using GraphQL.Types;
using Orbital.Schema;
using SimpleInjector;

namespace Orbital.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var connectionString = "Host=localhost;Username=orbital;Password=orbital;Database=orbital";

            DatabaseMigrator.Migrate(connectionString);
            var container = ContainerBuilder.Build(connectionString);

            RunAsync(container);
        }

        private static async void RunAsync(Container container)
        {
            var query = @"
query {
    clubs {
        id,
        name,
    }
}";

            var documentExecuter = new DocumentExecuter();
            var executionResult = await documentExecuter.ExecuteAsync(_ =>
            {
                _.Schema = new RootSchema(x => Resolve(container, x));
                _.Query = query;
                _.UserContext = new UserContext(container);

                _.FieldMiddleware.Use(LoggingFieldMiddleware.Exec);
            }).ConfigureAwait(false);
            var json = new DocumentWriter(true).Write(executionResult);

            File.WriteAllText("output.json", json);
        }

        private static GraphType Resolve(Container container, Type arg)
        {
            Console.WriteLine("Resolving {0}", arg.FullName);

            return (GraphType)container.GetInstance(arg);
        }
    }
}
