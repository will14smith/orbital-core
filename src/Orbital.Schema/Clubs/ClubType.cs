using GraphQL.Types;
using Orbital.Models.Domain;
using Orbital.Schema.People;

namespace Orbital.Schema.Clubs
{
  public class ClubType : ObjectGraphType<Club>
  {
    public ClubType()
    {
      Field(x => x.Id).Description("The ID of the club");
      Field(x => x.Name).Description("The name of the club");

      Field<ListGraphType<PersonType>>(
        name: "people",
        description: "The people who have this club as their primary",
        resolve: context => context.ResolveService<Club, IPersonService>().GetByClub(context.Source)
      );
    }
  }
}