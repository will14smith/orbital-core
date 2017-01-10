using GraphQL.Types;
using Orbital.Models.Domain;
using Orbital.Schema.Common;

namespace Orbital.Schema.People
{
    public class PersonInputType : InputObjectGraphType
    {
        public PersonInputType()
        {
            Field<NonNullGraphType<StringGraphType>>(name: "club", description: "The id of the primary club that the person belongs to");

            Field<NonNullGraphType<StringGraphType>>(name: "name", description: "The name of the person");

            Field<NonNullGraphType<EnumType<Gender>>>(name: "gender", description: "The gender of the person");
            Field<EnumType<Bowstyle>>(name: "bowstyle", description: "The bowstyle of the person");

            Field<StringGraphType>(name: "archeryGBNumber", description: "The ArcheryGB number of the person");

            Field<DateGraphType>(name: "dateOfBirth", description: "The date when the person was born");
            Field<DateGraphType>(name: "dateStartedArchery", description: "The date when the person started archery");
        }
    }
}