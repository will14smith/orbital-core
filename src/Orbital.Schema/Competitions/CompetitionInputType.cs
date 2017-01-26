using GraphQL.Types;

namespace Orbital.Schema.Competitions
{
    public class CompetitionInputType : InputObjectGraphType
    {
        public CompetitionInputType()
        {
            Field<StringGraphType>(
                name: "name",
                description: "The name of the competition"
            );
            Field<StringGraphType>(
                name: "start",
                description: "The start date of the competition"
            );
            Field<StringGraphType>(
                name: "end",
                description: "The end date of the competition"
            );
            Field<ListGraphType<StringGraphType>>(
                name: "rounds",
                description: "The list of round ids for the competition"
            );
        }
    }
}