using System;
using System.Collections.Generic;
using System.Linq;
using Orbital.Data;
using Orbital.Data.Entities;
using Orbital.Models.Domain;

namespace Orbital.Web.Rounds
{
    internal class RoundService : IRoundService
    {
        private readonly OrbitalContext _ctx;

        public RoundService(OrbitalContext ctx)
        {
            _ctx = ctx;
        }

        public IReadOnlyCollection<Round> GetAll()
        {
            return _ctx.Rounds
                .Where(x => !x.Deleted)
                .Select(ToDomain)
                .ToList();
        }

        public RoundViewModel GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Guid Create(RoundInputModel input)
        {
            throw new NotImplementedException();
        }

        public void Update(Guid id, RoundInputModel input)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }
        
        private RoundEntity Find(Guid id)
        {
            var entity = _ctx.Rounds.Find(id);
            if (entity == null || entity.Deleted)
            {
                return null;
            }

            return entity;
        }
        private IReadOnlyCollection<RoundTargetEntity> GetTargets(RoundEntity round)
        {
            return round.Targets.Where(x => !x.Deleted).ToList();
        }

        private Round ToDomain(RoundEntity entity)
        {
            return new Round(
                entity.Id, entity.VariantOfId,
                entity.Category,entity.Name, entity.Indoor,
                GetTargets(entity).Select(ToDomain).ToList()
            );
        }
        private RoundTarget ToDomain(RoundTargetEntity entity)
        {
            return new RoundTarget(
                entity.Id, (ScoringType) entity.ScoringType,
                new Length(entity.DistanceValue, (LengthUnit) entity.DistanceUnit), 
                new Length(entity.FaceSizeValue, (LengthUnit) entity.FaceSizeUnit), 
                entity.ArrowCount
            );
        }

        private void PopulateEntity(RoundEntity entity, RoundInputModel round)
        {
            throw new NotImplementedException();
        }
        private void PopulateEntity(RoundTargetEntity entity, RoundTargetInputModel target)
        {
            throw new NotImplementedException();
        }
    }
}