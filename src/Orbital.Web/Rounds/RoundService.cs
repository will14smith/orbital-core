using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Orbital.Data;
using Orbital.Data.Entities;
using Orbital.Models;
using Orbital.Web.Helpers;

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
                .AsEnumerable()
                .Select(ToDomain)
                .ToList();
        }

        public RoundViewModel GetById(Guid id)
        {
            var entity = Find(id);
            if (entity == null)
            {
                return null;
            }

            return new RoundViewModel(
                ToDomain(entity),
                _ctx.GetVersionInfo<RoundEntity>(id)
            );

        }

        public Guid Create(RoundInputModel input)
        {
            var entity = new RoundEntity();
            PopulateEntity(entity, input);

            _ctx.Rounds.Add(entity);
            _ctx.SaveChanges();

            return entity.Id;
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
            var entity = _ctx.Rounds.Include(x => x.Targets).FirstOrDefault(x => x.Id == id);
            if (entity == null || entity.Deleted)
            {
                return null;
            }

            return entity;
        }
        private IReadOnlyCollection<RoundTargetEntity> GetTargets(RoundEntity round)
        {
            return round.Targets?
                       .Where(x => !x.Deleted)
                       .ToList() 
                   ?? new List<RoundTargetEntity>();
        }

        private Round ToDomain(RoundEntity entity)
        {
            return new Round(
                entity.Id, entity.VariantOfId,
                entity.Category, entity.Name, entity.Indoor,
                GetTargets(entity).Select(ToDomain).ToList()
            );
        }
        private RoundTarget ToDomain(RoundTargetEntity entity)
        {
            return new RoundTarget(
                entity.Id, (ScoringType)entity.ScoringType,
                new Length(entity.DistanceValue, (LengthUnit)entity.DistanceUnit),
                new Length(entity.FaceSizeValue, (LengthUnit)entity.FaceSizeUnit),
                entity.ArrowCount
            );
        }

        private static void PopulateEntity(RoundEntity entity, RoundInputModel round)
        {
            entity.VariantOfId = round.VariantOfId;

            entity.Category = round.Category;
            entity.Name = round.Name;
            entity.Indoor = round.Indoor;

            entity.Targets = entity.Targets ?? new List<RoundTargetEntity>();
            entity.Targets.Clear();

            foreach (var target in round.Targets)
            {
                var targetEntity = new RoundTargetEntity();
                PopulateEntity(targetEntity, target);

                entity.Targets.Add(targetEntity);
            }
        }
        private static void PopulateEntity(RoundTargetEntity entity, RoundTargetInputModel target)
        {
            entity.ScoringType = (int)target.ScoringType;

            entity.DistanceValue = target.DistanceValue;
            entity.DistanceUnit = (int)target.DistanceUnit;
            entity.FaceSizeValue = target.FaceSizeValue;
            entity.FaceSizeUnit = (int)target.FaceSizeUnit;

            entity.ArrowCount = target.ArrowCount;
        }
    }
}