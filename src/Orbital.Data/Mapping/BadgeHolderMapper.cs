using Orbital.Data.Entities;
using Orbital.Models.Domain;

namespace Orbital.Data.Mapping
{
    internal static class BadgeHolderMapper
    {
        public static BadgeHolder ToDomain(this BadgeHolderEntity entity)
        {
            return new BadgeHolder(entity.Id, entity.BadgeId, entity.PersonId, entity.AwardedOn, entity.ConfirmedOn, entity.MadeOn, entity.DeliveredOn);
        }
        public static BadgeHolderEntity ToEntity(this BadgeHolder entity)
        {
            return new BadgeHolderEntity
            {
                Id = entity.Id,

                BadgeId = entity.BadgeId,
                PersonId = entity.PersonId,

                AwardedOn = entity.AwardedOn,
                ConfirmedOn = entity.ConfirmedOn,
                MadeOn = entity.MadeOn,
                DeliveredOn = entity.DeliveredOn,
            };
        }
    }
}