using System;
using Orbital.Models;
using Orbital.Models.Extensions;

namespace Orbital.Web.People
{
    public class PersonInputModel
    {
        public PersonInputModel()
        {
        }
        public PersonInputModel(Person person)
        {
            ClubId = person.ClubId;
            
            Name = person.Name;

            Gender = person.Gender;
            Bowstyle = person.Bowstyle;
            ArcheryGBNumber = person.ArcheryGBNumber;
            DateOfBirth = person.DateOfBirth.ToDateTime();
            DateStartedArchery = person.DateStartedArchery.ToDateTime();
        }

        public Guid ClubId { get; set; }

        public string Name { get; set; }

        public Gender Gender { get; set; }
        public Bowstyle? Bowstyle { get; set; }
        public string ArcheryGBNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateStartedArchery { get; set; }
    }
}