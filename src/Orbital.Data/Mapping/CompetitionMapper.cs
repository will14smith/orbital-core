using System.Collections.Generic;
using System.Linq;
using Orbital.Data.Entities;
using Orbital.Models.Domain;

namespace Orbital.Data.Mapping
{
    internal static class CompetitionMapper
    {
        public static Competition ToDomain(this CompetitionEntity entity, IEnumerable<int> rounds)
        {
            return new Competition(entity.Id, entity.Name, entity.Start, entity.End, rounds.ToList());
        }
        public static CompetitionEntity ToEntity(this Competition entity)
        {
            return new CompetitionEntity
            {
                Id = entity.Id,

                Name = entity.Name,

                Start = entity.Start,
                End = entity.End
            };
        }
    }
}
