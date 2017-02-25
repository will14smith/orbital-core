using GraphQL.Types;
using Orbital.Models.Services;
using Orbital.Schema.Clubs;
using Orbital.Schema.Competitions;
using Orbital.Schema.People;
using Orbital.Schema.Rounds;

namespace Orbital.Schema
{
    public class RootQuery : ObjectGraphType
    {
        public RootQuery()
        {
            Field<ListGraphType<ClubType>>(
                name: "Clubs",
                resolve: context => context.ResolveService<object, IClubService>().GetRoot());
            Field<ListGraphType<PersonType>>(
                name: "People",
                resolve: context => context.ResolveService<object, IPersonService>().GetRoot());
            Field<ListGraphType<RoundType>>(
                name: "Rounds",
                resolve: context => context.ResolveService<object, IRoundService>().GetRoot());
            Field<ListGraphType<CompetitionType>>(
                name: "Competitions",
                resolve: context => context.ResolveService<object, ICompetitionService>().GetRoot());
        }
    }
}