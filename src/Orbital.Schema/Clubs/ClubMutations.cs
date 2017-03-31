using GraphQL.Types;
using Orbital.Models.Domain;
using Orbital.Models.Repositories;

namespace Orbital.Schema.Clubs
{
    public static class ClubMutations
    {
        public static void AddToRoot(ObjectGraphType mutation)
        {
            mutation.Field<ClubType>(
                name: "addClub",
                arguments: new QueryArguments(
                    new QueryArgument<ClubInputType> { Name = "club" }
                ),
                resolve: AddClubResolve
            );

            mutation.Field<ClubType>(
                name: "updateClub",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "id" },
                    new QueryArgument<ClubInputType> { Name = "club" }
                ),
                resolve: UpdateClubResolve
            );
        }

        internal static object AddClubResolve(ResolveFieldContext<object> ctx)
        {
            var input = ctx.GetArgument<Club>("club");

            var clubService = ctx.ResolveService<object, IClubRepository>();

            return clubService.Create(input);
        }

        internal static object UpdateClubResolve(ResolveFieldContext<object> ctx)
        {
            var id = ctx.GetArgument<int>("id");
            var input = ctx.GetArgument<Club>("club");

            var clubService = ctx.ResolveService<object, IClubRepository>();

            return clubService.Update(new Club(id, input.Name));
        }
    }
}
