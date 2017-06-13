using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orbital.Data.Entities
{
    [Table("handicap")]
    internal class HandicapEntity
    {
        public Guid Id { get; set; }

        public Guid PersonId { get; set; }
        public Guid? ScoreId { get; set; }

        public int Type { get; set; }
        public DateTime Date { get; set; }
        public int Value { get; set; }

        public bool Indoor { get; set; }
        public int Bowstyle { get; set; }

        [ForeignKey(nameof(PersonId))]
        public PersonEntity Person { get; set; }
        [ForeignKey(nameof(ScoreId))]
        public ScoreEntity Score { get; set; }
    }
}