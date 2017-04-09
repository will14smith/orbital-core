using System;
using Orbital.Models.Domain;

namespace Orbital.Web.People
{
    public class PersonInputModel
    {
        public int Id { get; set; }

        public int ClubId { get; set; }

        public string Name { get; set; }

        public Gender Gender { get; set; }
        public Bowstyle? Bowstyle { get; set; }
        public string ArcheryGBNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateStartedArchery { get; set; }
    }
}