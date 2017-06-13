using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orbital.Data.Entities
{
    [Table("club")]
    internal class ClubEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        [InverseProperty(nameof(PersonEntity.Club))]
        public List<PersonEntity> Members { get; set; }
    }
}
