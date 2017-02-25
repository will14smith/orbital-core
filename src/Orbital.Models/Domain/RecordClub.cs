using System;

namespace Orbital.Models.Domain
{
    public class RecordClub
    {
        public RecordClub(int id, int clubId, DateTime activeFrom, DateTime activeTo)
        {
            Id = id;

            ClubId = clubId;

            ActiveFrom = activeFrom;
            ActiveTo = activeTo;
        }

        public int Id { get; }

        public int ClubId { get; }

        public DateTime ActiveFrom { get; }
        public DateTime ActiveTo { get; }
    }
}