namespace Orbital.Models.Domain
{
    public class RecordRound
    {
        public RecordRound(int id, int roundId, int count, Skill? skill, Bowstyle? bowstyle, Gender? gender)
        {
            Id = id;

            RoundId = roundId;

            Count = count;
            Skill = skill;
            Bowstyle = bowstyle;
            Gender = gender;
        }

        public int Id { get; }

        public int RoundId { get; }

        // conditions
        public int Count { get; }
        public Skill? Skill { get; }
        public Bowstyle? Bowstyle { get; }
        public Gender? Gender { get; }
    }
}