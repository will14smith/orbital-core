using GraphQL.Types;

namespace Orbital.Schema
{
  public static class ContextExtensions
  {
    public static T ResolveService<TField, T>(this ResolveFieldContext<TField> context)
      where T : class
    {
      var userContext = (IUserContext)context.UserContext;

      return userContext.ResolveService<T>();
    }
  }
}