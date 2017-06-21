using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Orbital.Data.Entities
{
    [Table("badge_holder")]
    public class BadgeHolderEntity
    {
        public Guid Id { get; set; }

        public Guid BadgeId { get; set; }
        public Guid PersonId { get; set; }

        public DateTime AwardedOn { get; set; }
        public DateTime? ConfirmedOn { get; set; }
        public DateTime? MadeOn { get; set; }
        public DateTime? DeliveredOn { get; set; }

        public bool Deleted { get; set; }

        [ForeignKey(nameof(BadgeId))]
        public BadgeEntity Badge { get; set; }
        [ForeignKey(nameof(PersonId))]
        public PersonEntity Person { get; set; }
    }
}
