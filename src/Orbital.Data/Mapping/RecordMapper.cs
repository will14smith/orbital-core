using System.Collections.Generic;
using System.Linq;
using Orbital.Models.Domain;

namespace Orbital.Data.Mapping
{
    internal static class RecordMapper
    {
        public static Record ToDomain(this RecordEntity entity, IEnumerable<RecordClub> clubs, IEnumerable<RecordRound> rounds)
        {
            return new Record(entity.Id, entity.TeamSize, clubs.ToList(), rounds.ToList());
        }
        public static RecordEntity ToEntity(this Record domain)
        {
            return new RecordEntity
            {
                Id = domain.Id,

                TeamSize = domain.TeamSize,
            };
        }

        public static RecordClub ToDomain(this RecordClubEntity entity)
        {
            return new RecordClub(entity.ClubId, entity.ActiveFrom, entity.ActiveTo);
        }
        public static RecordClubEntity ToEntity(this RecordClub domain, int recordId)
        {
            return new RecordClubEntity
            {
                RecordId = recordId,
                ClubId = domain.ClubId,

                ActiveFrom = domain.ActiveFrom,
                ActiveTo = domain.ActiveTo
            };
        }

        public static RecordRound ToDomain(this RecordRoundEntity entity)
        {
            return new RecordRound(entity.RoundId, entity.Count, (Skill?)entity.Skill, (Bowstyle?)entity.Bowstyle, (Gender?)entity.Gender);
        }
        public static RecordRoundEntity ToEntity(this RecordRound domain, int recordId)
        {
            return new RecordRoundEntity
            {
                RecordId = recordId,
                RoundId = domain.RoundId,

                Count = domain.Count,
                Skill = (int?)domain.Skill,
                Bowstyle = (int?)domain.Bowstyle,
                Gender = (int?)domain.Gender
            };
        }
    }
}
