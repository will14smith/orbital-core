using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orbital.Data.Entities
{
    [Table("person")]
    internal class PersonEntity
    {
        public Guid Id { get; set; }

        public Guid ClubId { get; set; }

        public string Name { get; set; }

        public int Gender { get; set; }
        public int? Bowstyle { get; set; }
        public string ArcheryGBNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateStartedArchery { get; set; }

        [ForeignKey(nameof(ClubId))]
        public ClubEntity Club { get; set; }

        [InverseProperty(nameof(BadgeHolderEntity.Person))]
        public List<BadgeHolderEntity> HeldBadges { get; set; }
        [InverseProperty(nameof(HandicapEntity.Person))]
        public List<HandicapEntity> Handicaps { get; set; }
        [InverseProperty(nameof(ScoreEntity.Person))]
        public List<ScoreEntity> Scores { get; set; }
    }
}
