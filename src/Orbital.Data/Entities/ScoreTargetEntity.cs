using Dapper.Contrib.Extensions;

namespace Orbital.Data.Entities
{
    [Table("score_target")]
    class ScoreTargetEntity
    {
        public int Id { get; set; }

        public int ScoreId { get; set; }

        public decimal Score { get; set; }
        public decimal Golds { get; set; }
        public decimal Hits { get; set; }
    }
}
