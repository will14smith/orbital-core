using System;
using Dapper.Contrib.Extensions;

namespace Orbital.Data.Entities
{
    [Table("handicap")]
    class HandicapEntity
    {
        public int Id { get; set; }

        public int PersonId { get; set; }
        public int? ScoreId { get; set; }

        public int Type { get; set; }
        public DateTime Date { get; set; }
        public int Value { get; set; }

        public bool Indoor { get; set; }
        public int Bowstyle { get; set; }
    }
}
