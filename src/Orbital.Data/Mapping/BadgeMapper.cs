using Orbital.Data.Entities;
using Orbital.Models.Domain;

namespace Orbital.Data.Mapping
{
    internal static class BadgeMapper
    {
        public static Badge ToDomain(this BadgeEntity entity)
        {
            return new Badge(entity.Id, entity.Name, entity.Description, entity.Algorithm, entity.Category, entity.Multiple, entity.ImageUrl);
        }
        public static BadgeEntity ToEntity(this Badge entity)
        {
            return new BadgeEntity
            {
                Id = entity.Id,

                Name = entity.Name,
                Description = entity.Description,
                Algorithm = entity.Algorithm,
                Category = entity.Category,
                Multiple = entity.Multiple,
                ImageUrl = entity.ImageUrl,
            };
        }
    }
}
