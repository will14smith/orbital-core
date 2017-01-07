using GraphQL.Types;

namespace Orbital.Schema.Clubs
{
    public class ClubInputType : InputObjectGraphType
    {
        public ClubInputType()
        {
            Field<StringGraphType>(
                name: "name",
                description: "The name of the club"
            );
        }
    }
}