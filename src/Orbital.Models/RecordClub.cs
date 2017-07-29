using NodaTime;

namespace Orbital.Models
{
    public class RecordClub
    {
        public RecordClub(int clubId, Instant activeFrom, Instant activeTo)
        {
            ClubId = clubId;

            ActiveFrom = activeFrom;
            ActiveTo = activeTo;
        }

        public int ClubId { get; }

        public Instant ActiveFrom { get; }
        public Instant ActiveTo { get; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var other = obj as RecordClub;
            return other != null && Equals(other);
        }

        protected bool Equals(RecordClub other)
        {
            return ClubId == other.ClubId 
                && ActiveFrom.Equals(other.ActiveFrom) 
                && ActiveTo.Equals(other.ActiveTo);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = ClubId;
                hashCode = (hashCode * 397) ^ ActiveFrom.GetHashCode();
                hashCode = (hashCode * 397) ^ ActiveTo.GetHashCode();
                return hashCode;
            }
        }
    }
}