using System.Collections.Generic;
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

            Field<RoundType>(
                name: "parent",
                description: "The optional parent round that this round is a variant of",
                resolve: GetParent
            );
            Field<ListGraphType<RoundType>>(
                name: "variants",
                description: "The list of rounds that are variants of this round",
                resolve: GetVariants
            );
        }

        private static Round GetParent(ResolveFieldContext<Round> ctx)
        {
            var round = ctx.Source;
            if (!round.VariantOfId.HasValue)
            {
                return null;
            }

            var service = ctx.ResolveService<Round, IRoundService>();
            return service.GetById(round.VariantOfId.Value);
        }

        private static IReadOnlyCollection<Round> GetVariants(ResolveFieldContext<Round> ctx)
        {
            var service = ctx.ResolveService<Round, IRoundService>();
            return service.GetVariants(ctx.Source.Id);
        }
    }
}
