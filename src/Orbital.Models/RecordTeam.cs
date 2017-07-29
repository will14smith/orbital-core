using System;
using System.Collections.Generic;
using NodaTime;

namespace Orbital.Models
{
    public class RecordTeam
    {
        public RecordTeam(int id, int recordId, int clubId, int competitionId, Instant dateSet, Instant? dateConfirmed, Instant? dateBroken, Instant? dateRevoked, IReadOnlyCollection<RecordTeamMember> members)
        {
            Id = id;

            RecordId = recordId;
            ClubId = clubId;
            CompetitionId = competitionId;

            DateSet = dateSet;
            DateConfirmed = dateConfirmed;
            DateBroken = dateBroken;
            DateRevoked = dateRevoked;
            Members = members;
        }

        public int Id { get; }

        public int RecordId { get; }
        public int ClubId { get; }
        public int? CompetitionId { get; }

        public Instant DateSet { get; }
        public Instant? DateConfirmed { get; }
        public Instant? DateBroken { get; }
        public Instant? DateRevoked { get; }

        public IReadOnlyCollection<RecordTeamMember> Members { get; }
    }
}
