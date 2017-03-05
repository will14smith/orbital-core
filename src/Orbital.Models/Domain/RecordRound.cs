namespace Orbital.Models.Domain
{
    public class RecordRound
    {
        public RecordRound(int roundId, int count, Skill? skill, Bowstyle? bowstyle, Gender? gender)
        {
            RoundId = roundId;

            Count = count;
            Skill = skill;
            Bowstyle = bowstyle;
            Gender = gender;
        }

        public int RoundId { get; }

        // conditions
        public int Count { get; }
        public Skill? Skill { get; }
        public Bowstyle? Bowstyle { get; }
        public Gender? Gender { get; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            var other = obj as RecordRound;
            return other != null && Equals(other);
        }

        protected bool Equals(RecordRound other)
        {
            return RoundId == other.RoundId
                && Count == other.Count
                && Skill == other.Skill
                && Bowstyle == other.Bowstyle
                && Gender == other.Gender;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = RoundId;
                hashCode = (hashCode * 397) ^ Count;
                hashCode = (hashCode * 397) ^ Skill.GetHashCode();
                hashCode = (hashCode * 397) ^ Bowstyle.GetHashCode();
                hashCode = (hashCode * 397) ^ Gender.GetHashCode();
                return hashCode;
            }
        }
    }
}