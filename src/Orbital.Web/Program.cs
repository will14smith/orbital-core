using System;
using GraphQL;
using GraphQL.Http;
using GraphQL.Instrumentation;
using GraphQL.Types;
using Orbital.Data.Repositories;
using Orbital.Models.Domain;
using Orbital.Models.Repositories;
using Orbital.Schema;
using Orbital.Schema.Clubs;
using SimpleInjector;

namespace Orbital.Web
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var container = BuildContainer();

      var repo = container.GetInstance<IClubRepository>();

      repo.Create(new Club(1, "ICAC"));
      repo.Create(new Club(2, "ULAC"));
      repo.Create(new Club(3, "LA"));

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
        }
      ";

      var documentExecuter = new DocumentExecuter();
      var executionResult = await documentExecuter.ExecuteAsync(_ =>
      {
        _.Schema = new RootSchema(x => Resolve(container, x));
        _.Query = query;
        _.UserContext = new UserContext(container);

        _.FieldMiddleware.Use(LoggingFieldMiddleware.Exec);
      }).ConfigureAwait(false);
      var json = new DocumentWriter(true).Write(executionResult);

      Console.WriteLine(json);
    }

    private static Container BuildContainer()
    {
      var container = new Container();

      container.Register<IClubRepository, InMemoryClubRepository>(Lifestyle.Singleton);

      container.Register<IClubService, ClubServiceImpl>(Lifestyle.Transient);

      container.Verify();
      return container;
    }

    private static GraphType Resolve(Container container, System.Type arg)
    {
      Console.WriteLine("Resolving {0}", arg.FullName);

      return (GraphType)container.GetInstance(arg);
    }
  }

  class LoggingFieldMiddleware
  {
    public static FieldMiddlewareDelegate Exec(FieldMiddlewareDelegate next)
    {
      return context =>
      {
        Console.WriteLine("Entered {0}", context.FieldName);
        var result = next(context);

        Console.WriteLine("Exception: {0}", result.Exception);

        Console.WriteLine("Finished {0}", context.FieldName);
        return result;
      };
    }
  }
}
