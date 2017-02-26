using Dapper.Contrib.Extensions;

namespace Orbital.Data.Entities
{
    [Table("badge")]
    class BadgeEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Algorithm { get; set; }
        public string Category { get; set; }
        public bool Multiple { get; set; }
        public string ImageUrl { get; set; }
    }
}
