using GraphQL.Types;
using Orbital.Schema.Clubs;

namespace Orbital.Schema
{
  public class RootQuery : ObjectGraphType
  {
    public RootQuery()
    {
      Field<ListGraphType<ClubType>>(
        name: "clubs",
        resolve: context => context.ResolveService<object, IClubService>().GetRoot());
    }
  }
}