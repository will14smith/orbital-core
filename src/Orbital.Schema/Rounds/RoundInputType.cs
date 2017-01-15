using GraphQL.Types;

namespace Orbital.Schema.Rounds
{
    public class RoundInputType : InputObjectGraphType
    {
        public RoundInputType()
        {
            Field<StringGraphType>(name: "variantOfId", description: "The ID of the round that this round is a variant of");

            Field<NonNullGraphType<StringGraphType>>(name: "category", description: "The Category of the round");
            Field<NonNullGraphType<StringGraphType>>(name: "name", description: "The Name of the round");
            Field<BooleanGraphType>(name: "indoor", description: "If the round is shot indoors or outdoors");

            Field<ListGraphType<RoundTargetInputType>>(name: "targets", description: "A list of all the targets that are used in this round");
        }
    }
}