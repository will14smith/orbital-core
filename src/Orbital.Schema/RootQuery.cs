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
                name: "clubs",
                resolve: context => context.ResolveService<object, IClubService>().GetRoot());
            Field<ListGraphType<PersonType>>(
                name: "people",
                resolve: context => context.ResolveService<object, IPersonService>().GetRoot());
            Field<ListGraphType<RoundType>>(
                name: "rounds",
                resolve: context => context.ResolveService<object, IRoundService>().GetRoot());
            Field<ListGraphType<CompetitionType>>(
                name: "competitions",
                resolve: context => context.ResolveService<object, ICompetitionService>().GetRoot());
        }
    }
}