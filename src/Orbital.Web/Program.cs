using System;
using System.IO;
using GraphQL;
using GraphQL.Http;
using GraphQL.Instrumentation;
using GraphQL.Types;
using Orbital.Data.Repositories;
using Orbital.Models.Domain;
using Orbital.Models.Repositories;
using Orbital.Schema;
using Orbital.Schema.Clubs;
using Orbital.Schema.People;
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
      var query = GraphQL.Introspection.SchemaIntrospection.IntrospectionQuery;

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

    private static Container BuildContainer()
    {
      var container = new Container();

      container.Register<IClubRepository, InMemoryClubRepository>(Lifestyle.Singleton);
      container.Register<IPersonRepository, InMemoryPersonRepository>(Lifestyle.Singleton);

      container.Register<IClubService, ClubServiceImpl>(Lifestyle.Transient);
      container.Register<IPersonService, PersonServiceImpl>(Lifestyle.Transient);

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
        try
        {
          var result = next(context);

          if (result?.Exception != null)
          {
            Console.WriteLine("Exception: {0}", result.Exception);
          }

          return result;
        }
        catch (Exception ex)
        {
          Console.WriteLine("Exception: {0}", ex);
        }
        finally
        {
          Console.WriteLine("Finished {0}", context.FieldName);
        }

        return null;
      };
    }
  }
}
