using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orbital.Data.Entities
{
    [Table("score_target")]
    public class ScoreTargetEntity
    {
        public ScoreTargetEntity()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        [ForeignKey(nameof(ScoreId))]
        public ScoreEntity Score { get; set; }
        public Guid ScoreId { get; set; }
        
        [ForeignKey(nameof(RoundTargetId))]
        public RoundTargetEntity RoundTarget { get; set; }
        public Guid RoundTargetId { get; set; }

        [Column("Score")]
        public decimal ScoreValue { get; set; }
        public decimal Golds { get; set; }
        public decimal Hits { get; set; }

        public bool Deleted { get; set; }
    }
}
