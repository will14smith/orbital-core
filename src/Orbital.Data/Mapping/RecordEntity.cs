using Dapper.Contrib.Extensions;

namespace Orbital.Data.Mapping
{
    [Table("record")]
    class RecordEntity
    {
        public int Id { get; set; }

        public int TeamSize { get; set; }
    }
}
