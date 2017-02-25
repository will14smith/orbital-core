using System.Collections.Generic;
using System.Linq;
using Orbital.Data.Entities;
using Orbital.Models.Domain;

namespace Orbital.Data.Mapping
{
    static class RoundMapper
    {
        public static Round ToDomain(this RoundEntity entity, IEnumerable<RoundTarget> targets)
        {
            return new Round(entity.Id, entity.VariantOfId, entity.Category, entity.Name, entity.Indoor, targets.ToList());
        }
        public static RoundEntity ToEntity(this Round domain)
        {
            return new RoundEntity
            {
                Id = domain.Id,

                VariantOfId = domain.VariantOfId,

                Category = domain.Category,
                Name = domain.Name,
                Indoor = domain.Indoor
            };
        }

        public static RoundTarget ToDomain(this RoundTargetEntity entity)
        {
            return new RoundTarget(entity.Id, (ScoringType)entity.ScoringType, new Length(entity.DistanceValue, (LengthUnit)entity.DistanceUnit), new Length(entity.FaceSizeValue, (LengthUnit)entity.FaceSizeUnit), entity.ArrowCount);
        }
        public static RoundTargetEntity ToEntity(this RoundTarget domain, int roundId)
        {
            return new RoundTargetEntity
            {
                Id = domain.Id,

                RoundId = roundId,

                ScoringType = (int)domain.ScoringType,

                DistanceValue = domain.Distance.Value,
                DistanceUnit = (int)domain.Distance.Unit,
                FaceSizeValue = domain.FaceSize.Value,
                FaceSizeUnit = (int)domain.FaceSize.Unit,

                ArrowCount = domain.ArrowCount
            };
        }
    }
}
