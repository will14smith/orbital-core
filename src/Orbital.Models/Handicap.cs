using System;

namespace Orbital.Models.Domain
{
    public class Handicap
    {
        public Handicap(int id, Handicap handicap)
            : this(
                id: id,

                personId: handicap.PersonId,
                scoreId: handicap.ScoreId,
                type: handicap.Type,
                date: handicap.Date,
                value: handicap.Value,
                identifier: handicap.Identifier
            )
        {
        }

        public Handicap(int id, int personId, int? scoreId, HandicapType type, DateTime date, int value, HandicapIdentifier identifier)
        {
            Id = id;
            PersonId = personId;
            ScoreId = scoreId;
            Type = type;
            Date = date;
            Value = value;
            Identifier = identifier;
        }

        public int Id { get; }

        public int PersonId { get; }
        public int? ScoreId { get; }

        public HandicapType Type { get; }
        public DateTime Date { get; }
        public int Value { get; }

        public HandicapIdentifier Identifier { get; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var other = obj as Handicap;
            return other != null && Equals(other);
        }

        protected bool Equals(Handicap other)
        {
            return Id == other.Id
                && PersonId == other.PersonId
                && ScoreId == other.ScoreId
                && Type == other.Type
                && Date.Equals(other.Date)
                && Value == other.Value
                && Equals(Identifier, other.Identifier);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id;
                hashCode = (hashCode * 397) ^ PersonId;
                hashCode = (hashCode * 397) ^ ScoreId.GetHashCode();
                hashCode = (hashCode * 397) ^ (int)Type;
                hashCode = (hashCode * 397) ^ Date.GetHashCode();
                hashCode = (hashCode * 397) ^ Value;
                hashCode = (hashCode * 397) ^ (Identifier?.GetHashCode() ?? 0);
                return hashCode;
            }
        }
    }
}
