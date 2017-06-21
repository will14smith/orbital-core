using System.Collections.Generic;
using System.Linq;

namespace Orbital.Models.Domain
{
    public class Record
    {
        public Record(int id, int teamSize, IReadOnlyCollection<RecordClub> clubs, IReadOnlyCollection<RecordRound> rounds)
        {
            Id = id;

            TeamSize = teamSize;

            Clubs = clubs;
            Rounds = rounds;
        }

        public int Id { get; }

        public int TeamSize { get; }

        public IReadOnlyCollection<RecordClub> Clubs { get; }
        public IReadOnlyCollection<RecordRound> Rounds { get; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var other = obj as Record;
            return other != null && Equals(other);
        }

        protected bool Equals(Record other)
        {
            return Id == other.Id
                && TeamSize == other.TeamSize
                && Clubs.SequenceEqual(other.Clubs)
                && Rounds.SequenceEqual(other.Rounds);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id;
                hashCode = (hashCode * 397) ^ TeamSize;
                hashCode = (hashCode * 397) ^ (Clubs?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (Rounds?.GetHashCode() ?? 0);
                return hashCode;
            }
        }
    }
}
