using GraphQL.Types;
using Orbital.Models.Domain;
using Orbital.Schema.Clubs;

namespace Orbital.Schema.People
{
  public class PersonType : ObjectGraphType<Person>
  {
    public PersonType()
    {
      Field(x => x.Id).Description("The ID of the person");
      Field<NonNullGraphType<ClubType>>(
        name: "club",
        description: "The primary club that the person belongs to",
        resolve: context => context.ResolveService<Person, IClubService>().GetById(context.Source.ClubId)
      );

      Field(x => x.Name).Description("The name of the person");

      Field<NonNullGraphType<EnumType<Gender>>>(
        name: "gender",
        description: "The gender of the person",
        resolve: context => context.Source.Gender
      );
      Field<EnumType<Bowstyle>>(
        name: "bowstyle",
        description: "The bowstyle of the person",
        resolve: context => context.Source.Bowstyle
      );

      Field(x => x.ArcheryGBNumber, nullable: true).Description("The ArcheryGB number of the person");

      Field(x => x.DateOfBirth, nullable: true).Description("The date when the person was born");
      Field(x => x.DateStartedArchery, nullable: true).Description("The date when the person started archery");
    }
  }
}