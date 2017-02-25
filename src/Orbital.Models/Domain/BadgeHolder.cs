using System;

namespace Orbital.Models.Domain
{
    public class BadgeHolder
    {
        public BadgeHolder(int id, BadgeHolder holder)
            : this(
                id: id,

                badgeId: holder.BadgeId,
                personId: holder.PersonId,

                awardedOn: holder.AwardedOn,
                confirmedOn: holder.ConfirmedOn,
                madeOn: holder.MadeOn,
                deliveredOn: holder.DeliveredOn
            )
        {
        }
        public BadgeHolder(int id, int badgeId, int personId, DateTime awardedOn, DateTime? confirmedOn, DateTime? madeOn, DateTime? deliveredOn)
        {
            Id = id;

            BadgeId = badgeId;
            PersonId = personId;

            AwardedOn = awardedOn;
            ConfirmedOn = confirmedOn;
            MadeOn = madeOn;
            DeliveredOn = deliveredOn;
        }

        public int Id { get; }

        public int BadgeId { get; }
        public int PersonId { get; }

        public DateTime AwardedOn { get; }
        public DateTime? ConfirmedOn { get; }
        public DateTime? MadeOn { get; }
        public DateTime? DeliveredOn { get; }
    }
}
