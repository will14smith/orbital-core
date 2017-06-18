using System;

namespace Orbital.Models.Domain
{
    public class Person
    {
        public Person(Guid id, Person person)
            : this(
                id: id,
                clubId: person.ClubId,
                name: person.Name,
                gender: person.Gender,
                bowstyle: person.Bowstyle,
                archeryGBNumber: person.ArcheryGBNumber,
                dateOfBirth: person.DateOfBirth,
                dateStartedArchery: person.DateStartedArchery)
        {

        }

        public Person(
            Guid id, Guid clubId,
            string name,
            Gender gender, Bowstyle? bowstyle = null,
            string archeryGBNumber = null,
            DateTime? dateOfBirth = null, DateTime? dateStartedArchery = null
          )
        {
            Id = id;
            ClubId = clubId;

            Name = name;

            Gender = gender;
            Bowstyle = bowstyle;

            ArcheryGBNumber = archeryGBNumber;

            DateOfBirth = dateOfBirth;
            DateStartedArchery = dateStartedArchery;
        }

        public Guid Id { get; }
        public Guid ClubId { get; }

        public string Name { get; }

        public Gender Gender { get; }
        public Bowstyle? Bowstyle { get; }
        public string ArcheryGBNumber { get; }
        public DateTime? DateOfBirth { get; }
        public DateTime? DateStartedArchery { get; }

        public override bool Equals(object obj)
        {
            var other = obj as Person;
            if (other == null) return false;

            return Id == other.Id
                && ClubId == other.ClubId
                && Name == other.Name
                && Gender == other.Gender
                && Bowstyle == other.Bowstyle
                && ArcheryGBNumber == other.ArcheryGBNumber
                && DateOfBirth == other.DateOfBirth
                && DateStartedArchery == other.DateStartedArchery;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode += ClubId.GetHashCode() * 317;
                hashCode += Name.GetHashCode() * 317;
                hashCode += Gender.GetHashCode() * 317;
                hashCode += (Bowstyle != null ? Bowstyle.GetHashCode() * 317 : 0);
                hashCode += (ArcheryGBNumber != null ? ArcheryGBNumber.GetHashCode() * 317 : 0);
                hashCode += (DateOfBirth != null ? DateOfBirth.GetHashCode() * 317 : 0);
                hashCode += (DateStartedArchery != null ? DateStartedArchery.GetHashCode() * 317 : 0);

                return hashCode;
            }
        }
    }
}
