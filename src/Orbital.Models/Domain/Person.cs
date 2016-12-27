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
    }
}
