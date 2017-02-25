using System;

namespace Orbital.Data.Entities
{
    class PersonEntity
    {
        public int Id { get; set; }

        public int ClubId { get; set; }

        public string Name { get; set; }

        public int Gender { get; set; }
        public int? Bowstyle { get; set; }
        public string ArcheryGBNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateStartedArchery { get; set; }
    }
}
