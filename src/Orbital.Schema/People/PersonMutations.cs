using GraphQL.Types;
using Orbital.Models.Domain;
using Orbital.Models.Services;

namespace Orbital.Schema.People
{
    public static class PersonMutations
    {
        public static void AddToRoot(ObjectGraphType mutation)
        {
            mutation.Field<PersonType>(
                name: "addPerson",
                arguments: new QueryArguments(
                    new QueryArgument<PersonInputType> { Name = "person" }
                ),
                resolve: AddPersonResolve
            );

            mutation.Field<PersonType>(
                name: "updatePerson",
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "id" },
                    new QueryArgument<PersonInputType> { Name = "person" }
                ),
                resolve: UpdatePersonResolve
            );
        }

        internal static object AddPersonResolve(ResolveFieldContext<object> ctx)
        {
            var input = ctx.GetArgument<Person>("person");

            var personService = ctx.ResolveService<object, IPersonService>();

            return personService.Add(input);
        }

        internal static object UpdatePersonResolve(ResolveFieldContext<object> ctx)
        {
            var id = ctx.GetArgument<int>("id");
            var input = ctx.GetArgument<Person>("person");

            var personService = ctx.ResolveService<object, IPersonService>();

            return personService.Update(id, input);
        }
    }
}
