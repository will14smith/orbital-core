namespace Orbital.Schema
{
  public interface IUserContext
  {
    T ResolveService<T>() where T : class;
  }
}