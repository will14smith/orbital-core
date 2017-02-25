using Orbital.Data.Entities;
using Orbital.Models.Domain;

namespace Orbital.Data.Mapping
{
    internal static class ClubMapper
    {
        public static Club ToDomain(this ClubEntity entity)
        {
            return new Club(entity.Id, entity.Name);
        }
        public static ClubEntity ToEntity(this Club entity)
        {
            return new ClubEntity
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }
    }
}
