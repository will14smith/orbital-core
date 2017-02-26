using Dapper.Contrib.Extensions;

namespace Orbital.Data.Entities
{
    [Table("round")]
    class RoundEntity
    {
        public int Id { get; set; }

        public int? VariantOfId { get; set; }

        public string Category { get; set; }
        public string Name { get; set; }
        public bool Indoor { get; set; }
    }
}
