using GraphQL.Types;
using Orbital.Models.Domain;
using Orbital.Schema.Common;

namespace Orbital.Schema.Rounds
{
    public class RoundTargetInputType : InputObjectGraphType
    {
        public RoundTargetInputType()
        {
            Field<NonNullGraphType<EnumType<ScoringType>>>(name: "scoringType", description: "The type of scoring used on this target");

            Field<LengthType>(name: "distance", description: "The distance this target is placed at");
            Field<LengthType>(name: "faceSize", description: "The size of target face used on this target");

            Field<IntGraphType>(name: "arrowCount", description: "The amount of arrows shot at this target");
        }
    }
}