using System;
using Dapper.Contrib.Extensions;

namespace Orbital.Data.Entities
{
    [Table("person")]
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
