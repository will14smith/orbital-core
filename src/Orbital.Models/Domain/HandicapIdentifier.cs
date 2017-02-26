namespace Orbital.Models.Domain
{
    public class HandicapIdentifier
    {
        public HandicapIdentifier(bool indoor, Bowstyle bowstyle)
        {
            Indoor = indoor;
            Bowstyle = bowstyle;
        }

        public bool Indoor { get; }
        public Bowstyle Bowstyle { get; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var other = obj as HandicapIdentifier;
            return other != null && Equals(other);
        }

        protected bool Equals(HandicapIdentifier other)
        {
            return Indoor == other.Indoor && Bowstyle == other.Bowstyle;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Indoor.GetHashCode() * 397) ^ (int)Bowstyle;
            }
        }
    }
}