using System;
using NodaTime;

namespace Orbital.Models
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
            LocalDate? dateOfBirth = null, LocalDate? dateStartedArchery = null
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
        public LocalDate? DateOfBirth { get; }
        public LocalDate? DateStartedArchery { get; }

        public decimal? GetAge(LocalDate? now = null)
        {
            if (!DateOfBirth.HasValue)
            {
                return null;
            }

            var from = now ?? SystemClock.Instance.GetCurrentInstant().InUtc().Date;

            var period = Period.Between(DateOfBirth.Value, from, PeriodUnits.Years | PeriodUnits.Days);

            return period.Years + Math.Round(period.Days / 365m, 1);
        }
        public Skill GetSkill(LocalDate? now = null)
        {
            if (!DateStartedArchery.HasValue)
            {
                return Skill.Experienced;
            }

            var from = now ?? SystemClock.Instance.GetCurrentInstant().InUtc().Date;

            var period = Period.Between(DateStartedArchery.Value, from, PeriodUnits.Years);

            return period.Years >= 1 ? Skill.Experienced : Skill.Novice;
        }

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
