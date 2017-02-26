using Dapper.Contrib.Extensions;

namespace Orbital.Data.Entities
{
    [Table("competition_round")]
    class CompetitionRoundEntity
    {
        public int CompetitionId { get; set; }
        public int RoundId { get; set; }
    }
}
