using Orbital.Schema;
using SimpleInjector;

namespace Orbital.Web
{
  class UserContext : IUserContext
  {
    private Container _container;

    public UserContext(Container container)
    {
      _container = container;
    }

    public T ResolveService<T>()
      where T : class
    {
      return _container.GetInstance<T>();
    }
  }
}