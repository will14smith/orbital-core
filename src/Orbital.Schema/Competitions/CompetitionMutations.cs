using GraphQL.Types;
using Orbital.Models.Domain;

namespace Orbital.Schema.Competitions
{
    public static class CompetitionMutations
    {
        public static void AddToRoot(ObjectGraphType mutation)
        {
            mutation.Field<CompetitionType>(
                name: "addCompetition",
                arguments: new QueryArguments(
                    new QueryArgument<CompetitionInputType> { Name = "competition" }
                ),
                resolve: AddRoundResolve
            );

            mutation.Field<CompetitionType>(
                name: "updateCompetition",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "id" },
                    new QueryArgument<CompetitionInputType> { Name = "competition" }
                ),
                resolve: UpdateRoundResolve
            );
        }

        internal static object AddRoundResolve(ResolveFieldContext<object> ctx)
        {
            var input = ctx.GetArgument<Competition>("competition");

            var competitionService = ctx.ResolveService<object, ICompetitionService>();

            return competitionService.Add(input);
        }

        internal static object UpdateRoundResolve(ResolveFieldContext<object> ctx)
        {
            var id = ctx.GetArgument<int>("id");
            var input = ctx.GetArgument<Competition>("competition");

            var competitionService = ctx.ResolveService<object, ICompetitionService>();

            return competitionService.Update(id, input);
        }
    }
}
