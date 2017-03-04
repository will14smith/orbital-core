using Orbital.Data.Entities;
using Orbital.Models.Domain;

namespace Orbital.Data.Mapping
{
    internal static class HandicapMapper
    {
        public static Handicap ToDomain(this HandicapEntity entity)
        {
            var ident = new HandicapIdentifier(entity.Indoor, (Bowstyle)entity.Bowstyle);

            return new Handicap(entity.Id, entity.PersonId, entity.ScoreId, (HandicapType)entity.Type, entity.Date, entity.Value, ident);
        }
        public static HandicapEntity ToEntity(this Handicap entity)
        {
            return new HandicapEntity
            {
                Id = entity.Id,

                PersonId = entity.PersonId,
                ScoreId = entity.ScoreId,

                Type = (int)entity.Type,
                Date = entity.Date,
                Value = entity.Value,

                Indoor = entity.Identifier.Indoor,
                Bowstyle = (int)entity.Identifier.Bowstyle
            };
        }
    }
}