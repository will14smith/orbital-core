using GraphQL.Types;
using Orbital.Models.Domain;

namespace Orbital.Schema.Common
{
    public class LengthType : ObjectGraphType<Length>
    {
        public LengthType()
        {
            Field(x => x.Value).Description("The value of the length");

            Field<NonNullGraphType<EnumType<LengthUnit>>>(
                name: "unit",
                description: "The unit of the value",
                resolve: context => context.Source.Unit
            );
        }
    }
}
