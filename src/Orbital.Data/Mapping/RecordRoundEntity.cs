using Dapper.Contrib.Extensions;

namespace Orbital.Data.Mapping
{
    [Table("record_round")]
    class RecordRoundEntity
    {
        public int Id { get; set; }

        public int RecordId { get; set; }
        public int RoundId { get; set; }

        public int Count { get; set; }
        public int? Skill { get; set; }
        public int? Bowstyle { get; set; }
        public int? Gender { get; set; }
    }
}