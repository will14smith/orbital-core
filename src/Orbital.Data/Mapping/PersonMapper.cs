using Orbital.Data.Entities;
using Orbital.Models.Domain;

namespace Orbital.Data.Mapping
{
    static class PersonMapper
    {
        public static Person ToDomain(this PersonEntity entity)
        {
            return new Person(entity.Id, entity.ClubId, entity.Name, (Gender) entity.Gender, (Bowstyle?) entity.Bowstyle, entity.ArcheryGBNumber, entity.DateOfBirth, entity.DateStartedArchery);
        }
        public static PersonEntity ToEntity(this Person domain)
        {
            return new PersonEntity
            {
                Id = domain.Id,

                ClubId = domain.ClubId,

                Name = domain.Name,

                Gender = (int) domain.Gender,
                Bowstyle = (int?) domain.Bowstyle,
                ArcheryGBNumber = domain.ArcheryGBNumber,
                DateOfBirth = domain.DateOfBirth,
                DateStartedArchery = domain.DateStartedArchery
            };
        }
    }
}
