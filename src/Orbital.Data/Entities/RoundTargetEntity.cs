using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orbital.Data.Entities
{
    [Table("round_target")]
    public class RoundTargetEntity
    {
        public RoundTargetEntity()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public Guid RoundId { get; set; }

        public int ScoringType { get; set; }

        public decimal DistanceValue { get; set; }
        public int DistanceUnit { get; set; }
        public decimal FaceSizeValue { get; set; }
        public int FaceSizeUnit { get; set; }

        public int ArrowCount { get; set; }

        public bool Deleted { get; set; }

        [ForeignKey(nameof(RoundId))]
        public RoundEntity Round { get; set; }
    }
}
