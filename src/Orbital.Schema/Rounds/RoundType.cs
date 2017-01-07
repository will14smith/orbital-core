using GraphQL.Types;
using Orbital.Models.Domain;

namespace Orbital.Schema.Rounds
{
    public class RoundType : ObjectGraphType<Round>
    {
        public RoundType()
        {
            Field(x => x.Id).Description("The ID of the round");

            Field(x => x.VariantOfId, nullable: true).Description("The ID of the round that this round is a variant of");

            Field(x => x.Category).Description("The Category of the round");
            Field(x => x.Name).Description("The Name of the round");
            Field(x => x.Indoor).Description("If the round is shot indoors or outdoors");

            Field<ListGraphType<RoundTargetType>>(
                name: "targets",
                description: "A list of all the targets that are used in this round",
                resolve: x => x.Source.Targets
            );
        }
    }
}
