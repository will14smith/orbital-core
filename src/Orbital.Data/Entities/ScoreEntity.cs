using System;
using Dapper.Contrib.Extensions;

namespace Orbital.Data.Entities
{
    [Table("score")]
    class ScoreEntity
    {
        public int Id { get; set; }

        public int PersonId { get; set; }
        public int ClubId { get; set; }
        public int RoundId { get; set; }
        public int? CompetitionId { get; set; }

        public int Bowstyle { get; set; }

        public decimal TotalScore { get; set; }
        public decimal TotalGolds { get; set; }
        public decimal TotalHits { get; set; }

        public DateTime ShotAt { get; set; }
        public DateTime EnteredAt { get; set; }
    }
}
