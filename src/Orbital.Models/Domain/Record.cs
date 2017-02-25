using System.Collections.Generic;

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
    }
}
