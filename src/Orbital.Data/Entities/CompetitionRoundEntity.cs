using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orbital.Data.Entities
{
    [Table("competition_round")]
    internal class CompetitionRoundEntity
    {
        public Guid CompetitionId { get; set; }
        public Guid RoundId { get; set; }

        [ForeignKey(nameof(CompetitionId))]
        public CompetitionEntity Competition { get; set; }
        [ForeignKey(nameof(RoundId))]
        public RoundEntity Round { get; set; }
    }
}
