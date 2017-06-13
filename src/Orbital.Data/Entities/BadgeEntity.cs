using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orbital.Data.Entities
{
    [Table("badge")]
    internal class BadgeEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Algorithm { get; set; }
        public string Category { get; set; }
        public bool Multiple { get; set; }
        public string ImageUrl { get; set; }

        [InverseProperty(nameof(BadgeHolderEntity.Badge))]
        public List<BadgeHolderEntity> BadgeHolders { get; set; }
    }
}
