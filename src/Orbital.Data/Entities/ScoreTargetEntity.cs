using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orbital.Data.Entities
{
    [Table("score_target")]
    class ScoreTargetEntity
    {
        public Guid Id { get; set; }

        [ForeignKey(nameof(ScoreId))]
        public ScoreEntity Score { get; set; }
        public Guid ScoreId { get; set; }

        [Column("Score")]
        public decimal ScoreValue { get; set; }
        public decimal Golds { get; set; }
        public decimal Hits { get; set; }
    }
}
