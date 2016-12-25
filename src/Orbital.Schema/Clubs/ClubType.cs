using GraphQL.Types;
using Orbital.Models.Domain;

namespace Orbital.Schema.Clubs
{
  public class ClubType : ObjectGraphType<Club>
  {
    public ClubType()
    {
      Field(x => x.Id).Description("The ID of the club");
      Field(x => x.Name).Description("The name of the club");
    }
  }
}