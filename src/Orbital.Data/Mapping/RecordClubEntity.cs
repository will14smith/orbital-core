using System;
using Dapper.Contrib.Extensions;

namespace Orbital.Data.Mapping
{
    [Table("record_club")]
    class RecordClubEntity
    {
        public int Id { get; set; }

        public int RecordId { get; set; }
        public int ClubId { get; set; }

        public DateTime ActiveFrom { get; set; }
        public DateTime ActiveTo { get; set; }
    }
}