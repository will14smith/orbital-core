using Dapper.Contrib.Extensions;

namespace Orbital.Data.Entities
{
    [Table("round_target")]
    class RoundTargetEntity
    {
        public int Id { get; set; }

        public int RoundId { get; set; }

        public int ScoringType { get; set; }

        public decimal DistanceValue { get; set; }
        public int DistanceUnit { get; set; }
        public decimal FaceSizeValue { get; set; }
        public int FaceSizeUnit { get; set; }

        public int ArrowCount { get; set; }
    }
}
