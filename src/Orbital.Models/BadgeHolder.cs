using System;

namespace Orbital.Models
{
    public class BadgeHolder
    {
        public BadgeHolder(Guid id, BadgeHolder holder)
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
        public BadgeHolder(Guid id, Guid badgeId, Guid personId, DateTime awardedOn, DateTime? confirmedOn, DateTime? madeOn, DateTime? deliveredOn)
        {
            Id = id;

            BadgeId = badgeId;
            PersonId = personId;

            AwardedOn = awardedOn;
            ConfirmedOn = confirmedOn;
            MadeOn = madeOn;
            DeliveredOn = deliveredOn;
        }

        public Guid Id { get; }

        public Guid BadgeId { get; }
        public Guid PersonId { get; }

        public DateTime AwardedOn { get; }
        public DateTime? ConfirmedOn { get; }
        public DateTime? MadeOn { get; }
        public DateTime? DeliveredOn { get; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var other = obj as BadgeHolder;
            return other != null && Equals(other);
        }

        protected bool Equals(BadgeHolder other)
        {
            return Id == other.Id

                && BadgeId == other.BadgeId
                && PersonId == other.PersonId

                && AwardedOn.Equals(other.AwardedOn)
                && ConfirmedOn.Equals(other.ConfirmedOn)
                && MadeOn.Equals(other.MadeOn)
                && DeliveredOn.Equals(other.DeliveredOn);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ BadgeId.GetHashCode();
                hashCode = (hashCode * 397) ^ PersonId.GetHashCode();
                hashCode = (hashCode * 397) ^ AwardedOn.GetHashCode();
                hashCode = (hashCode * 397) ^ (ConfirmedOn?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (MadeOn?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (DeliveredOn?.GetHashCode() ?? 0);
                return hashCode;
            }
        }
    }
}
