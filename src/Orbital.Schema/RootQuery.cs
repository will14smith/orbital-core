using GraphQL.Types;
using Orbital.Schema.Clubs;
using Orbital.Schema.People;

namespace Orbital.Schema
{
  public class RootQuery : ObjectGraphType
  {
    public RootQuery()
    {
      Field<ListGraphType<ClubType>>(
        name: "clubs",
        resolve: context => context.ResolveService<object, IClubService>().GetRoot());
      Field<ListGraphType<PersonType>>(
        name: "people",
        resolve: context => context.ResolveService<object, IPersonService>().GetRoot());
    }
  }
}