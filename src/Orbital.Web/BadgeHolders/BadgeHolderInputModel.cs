using System;

namespace Orbital.Web.BadgeHolders
{
    public class BadgeHolderInputModel
    {
        public int Id { get; set; }

        public int BadgeId { get; set; }
        public int PersonId { get; set; }

        public DateTime AwardedOn { get; set; }
        public DateTime? ConfirmedOn { get; set; }
        public DateTime? MadeOn { get; set; }
        public DateTime? DeliveredOn { get; set; }
    }
}
