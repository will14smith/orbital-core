using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orbital.Data.Entities
{
    [Table("competition")]
    public class CompetitionEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTimeOffset Start { get; set; }
        public DateTimeOffset End { get; set; }

        public bool Deleted { get; set; }

        [InverseProperty(nameof(CompetitionRoundEntity.Competition))]
        public List<CompetitionRoundEntity> Rounds { get; set; }

        [InverseProperty(nameof(ScoreEntity.Competition))]
        public List<ScoreEntity> Scores { get; set; }
    }
}
