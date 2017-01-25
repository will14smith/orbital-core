using GraphQL.Types;
using Orbital.Models.Domain;
using Orbital.Schema.Rounds;

namespace Orbital.Schema.Competitions
{
  public class CompetitionType : ObjectGraphType<Competition>
  {
    public CompetitionType()
    {
      Field(x => x.Id).Description("The ID of the competition");
      Field(x => x.Name).Description("The name of the competition");
      Field(x => x.Start).Description("The start time of the competition");
      Field(x => x.End).Description("The end time of the competition");

      Field<ListGraphType<RoundType>>(
        name: "rounds",
        description: "Rounds shot at this comptition",
        resolve: context => context.ResolveService<Competition, IRoundService>().GetByCompetition(context.Source)
      );
    }
  }
}