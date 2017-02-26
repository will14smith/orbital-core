using Dapper.Contrib.Extensions;

namespace Orbital.Data.Entities
{
    [Table("club")]
    class ClubEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
