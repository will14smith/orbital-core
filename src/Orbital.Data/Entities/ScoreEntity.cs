using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orbital.Data.Entities
{
    [Table("score")]
    public class ScoreEntity
    {
        public ScoreEntity()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public Guid PersonId { get; set; }
        public Guid ClubId { get; set; }
        public Guid RoundId { get; set; }
        public Guid? CompetitionId { get; set; }

        public int Bowstyle { get; set; }

        public decimal TotalScore { get; set; }
        public decimal TotalGolds { get; set; }
        public decimal TotalHits { get; set; }

        public DateTime ShotAt { get; set; }

        public bool Deleted { get; set; }

        [ForeignKey(nameof(PersonId))]
        public PersonEntity Person { get; set; }
        [ForeignKey(nameof(ClubId))]
        public ClubEntity Club { get; set; }
        [ForeignKey(nameof(RoundId))]
        public RoundEntity Round { get; set; }
        [ForeignKey(nameof(CompetitionId))]
        public CompetitionEntity Competition { get; set; }

        [InverseProperty(nameof(ScoreTargetEntity.Score))]
        public List<ScoreTargetEntity> Targets { get; set; }

        [InverseProperty(nameof(HandicapEntity.Score))]
        public List<HandicapEntity> Handicaps { get; set; }
    }
}
