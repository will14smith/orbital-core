using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orbital.Data.Entities
{
    [Table("competition_round")]
    public class CompetitionRoundEntity
    {
        public Guid Id { get; set; }

        public Guid CompetitionId { get; set; }
        public Guid RoundId { get; set; }

        public bool Deleted { get; set; }

        [ForeignKey(nameof(CompetitionId))]
        public CompetitionEntity Competition { get; set; }
        [ForeignKey(nameof(RoundId))]
        public RoundEntity Round { get; set; }
    }
}
