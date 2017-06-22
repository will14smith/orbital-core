﻿using System;
using System.Collections.Generic;

namespace Orbital.Models.Domain
{
    public class RecordTeam
    {
        public RecordTeam(int id, int recordId, int clubId, int competitionId, DateTime dateSet, DateTime? dateConfirmed, DateTime? dateBroken, DateTime? dateRevoked, IReadOnlyCollection<RecordTeamMember> members)
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

        public DateTime DateSet { get; }
        public DateTime? DateConfirmed { get; }
        public DateTime? DateBroken { get; }
        public DateTime? DateRevoked { get; }

        public IReadOnlyCollection<RecordTeamMember> Members { get; }
    }
}