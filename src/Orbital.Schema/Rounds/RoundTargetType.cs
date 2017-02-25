using GraphQL.Types;
using Orbital.Models.Domain;
using Orbital.Schema.Common;

namespace Orbital.Schema.Rounds
{
    public class RoundTargetType : ObjectGraphType<RoundTarget>
    {
        public RoundTargetType()
        {
            Field(x => x.Id).Description("The ID of the round");

            Field<NonNullGraphType<EnumType<ScoringType>>>(
                name: "ScoringType",
                description: "The type of scoring used on this target",
                resolve: context => context.Source.ScoringType
            );


            Field<LengthType>(
                name: "Distance",
                description: "The distance this target is placed at",
                resolve: context => context.Source.Distance
            );
            Field<LengthType>(
                name: "FaceSize",
                description: "The size of target face used on this target",
                resolve: context => context.Source.FaceSize
            );

            Field(x => x.ArrowCount).Description("The amount of arrows shot at this target");
        }
    }
}
