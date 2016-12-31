using System;

namespace Orbital.Models.Domain
{
    public class Person
    {
        public Person(int id, Person person)
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
          int id, int clubId,
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

        public int Id { get; private set; }
        public int ClubId { get; private set; }

        public string Name { get; private set; }

        public Gender Gender { get; private set; }
        public Bowstyle? Bowstyle { get; private set; }
        public string ArcheryGBNumber { get; private set; }
        public DateTime? DateOfBirth { get; private set; }
        public DateTime? DateStartedArchery { get; private set; }

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
                var hashCode = Id;
                hashCode += ClubId * 317;
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
