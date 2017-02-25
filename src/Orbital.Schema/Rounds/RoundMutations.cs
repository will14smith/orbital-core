using GraphQL.Types;
using Orbital.Models.Domain;
using Orbital.Models.Services;

namespace Orbital.Schema.Rounds
{
    public static class RoundMutations
    {
        public static void AddToRoot(ObjectGraphType mutation)
        {
            mutation.Field<RoundType>(
                name: "addRound",
                arguments: new QueryArguments(
                    new QueryArgument<RoundInputType> { Name = "round" }
                ),
                resolve: AddRoundResolve
            );

            mutation.Field<RoundType>(
                name: "updateRound",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "id" },
                    new QueryArgument<RoundInputType> { Name = "round" }
                ),
                resolve: UpdateRoundResolve
            );
        }

        internal static object AddRoundResolve(ResolveFieldContext<object> ctx)
        {
            var input = ctx.GetArgument<Round>("round");

            var roundService = ctx.ResolveService<object, IRoundService>();

            return roundService.Add(input);
        }

        internal static object UpdateRoundResolve(ResolveFieldContext<object> ctx)
        {
            var id = ctx.GetArgument<int>("id");
            var input = ctx.GetArgument<Round>("round");

            var roundService = ctx.ResolveService<object, IRoundService>();

            return roundService.Update(id, input);
        }
    }
}
