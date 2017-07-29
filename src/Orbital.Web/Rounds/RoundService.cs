using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IReadOnlyCollection<Round>> GetAll()
        {
            var rounds = await _ctx.Rounds.Where(x => !x.Deleted).ToListAsync();

            return rounds.Select(ToDomain).ToList();
        }

        public async Task<RoundViewModel> GetById(Guid id)
        {
            var entity = await Find(id);
            if (entity == null)
            {
                return null;
            }

            var domain = ToDomain(entity);
            var versionInfo = await _ctx.GetVersionInfo<RoundEntity>(id);

            return new RoundViewModel(
                domain,
                versionInfo
            );

        }

        public async Task<Guid> Create(RoundInputModel input)
        {
            var entity = new RoundEntity();
            PopulateEntity(entity, input);

            _ctx.Rounds.Add(entity);
            await _ctx.SaveChangesAsync();

            return entity.Id;
        }

        public async Task Update(Guid id, RoundInputModel input)
        {
            var entity = await Find(id);
            if (entity == null)
            {
                throw new Exception($"Couldn't find club with id = {id} to update");
            }

            // TODO check for variant cycles
            
            PopulateEntity(entity, input);

            _ctx.Rounds.Update(entity);
            await _ctx.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var entity = await Find(id);
            if (entity == null)
            {
                throw new Exception($"Couldn't find club with id = {id} to delete");
            }

            // TODO check for non-deleted variants

            entity.Deleted = true;

            _ctx.Rounds.Update(entity);
            await _ctx.SaveChangesAsync();
        }

        private async Task<RoundEntity> Find(Guid id)
        {
            var entity = await _ctx.Rounds.Include(x => x.Targets).FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null || entity.Deleted)
            {
                return null;
            }

            return entity;
        }

        private static IEnumerable<RoundTargetEntity> GetTargets(RoundEntity round)
        {
            return round.Targets?.Where(x => !x.Deleted)
                   ?? new List<RoundTargetEntity>();
        }

        private static Round ToDomain(RoundEntity entity)
        {
            return new Round(
                entity.Id, entity.VariantOfId,
                entity.Category, entity.Name, entity.Indoor,
                GetTargets(entity).Select(ToDomain).ToList()
            );
        }
        private static RoundTarget ToDomain(RoundTargetEntity entity)
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

            if (round.Targets == null)
            {
                return;
            }
            
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