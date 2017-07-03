using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orbital.Data.Entities
{
    [Table("round")]
    public class RoundEntity : IEntity
    {
        public RoundEntity()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public Guid? VariantOfId { get; set; }

        public string Category { get; set; }
        public string Name { get; set; }
        public bool Indoor { get; set; }

        public bool Deleted { get; set; }

        [ForeignKey(nameof(VariantOfId))]
        public RoundEntity ParentRound { get; set; }
        [InverseProperty(nameof(ParentRound))]
        public List<RoundEntity> VariantRounds { get; set; }

        [InverseProperty(nameof(RoundTargetEntity.Round))]
        public List<RoundTargetEntity> Targets { get; set; }

        [InverseProperty(nameof(CompetitionRoundEntity.Round))]
        public List<CompetitionRoundEntity> Competitions { get; set; }
        [InverseProperty(nameof(ScoreEntity.Round))]
        public List<ScoreEntity> Scores { get; set; }
    }
}
