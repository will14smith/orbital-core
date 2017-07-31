using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NodaTime.Extensions;
using Orbital.Data;
using Orbital.Data.Entities;
using Orbital.Models;

namespace Orbital.Web.Scores
{
    public class ScoreService : IScoreService
    {
        private readonly OrbitalContext _ctx;

        public ScoreService(OrbitalContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<IReadOnlyCollection<Score>> GetAll()
        {
            var rounds = await _ctx.Scores.Where(x => !x.Deleted).ToListAsync();

            return rounds.Select(ToDomain).ToList();
        }

        public Task<ScoreViewModel> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> Create(ScoreInputModel input)
        {
            throw new NotImplementedException();
        }

        public Task Update(Guid id, ScoreInputModel input)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        private async Task<ScoreEntity> Find(Guid id)
        {
            var entity = await _ctx.Scores.Include(x => x.Targets).FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null || entity.Deleted)
            {
                return null;
            }

            return entity;
        }
        private static IEnumerable<ScoreTargetEntity> GetTargets(ScoreEntity score)
        {
            return score.Targets?.Where(x => !x.Deleted)
                   ?? new List<ScoreTargetEntity>();
        }

        private static Score ToDomain(ScoreEntity entity)
        {
            return new Score(
                entity.Id,
                entity.PersonId, entity.ClubId, entity.RoundId, entity.CompetitionId,
                (Bowstyle) entity.Bowstyle, 
                entity.TotalScore, entity.TotalGolds, entity.TotalHits,
                entity.ShotAt.ToInstant(),
                GetTargets(entity).Select(ToDomain).ToList()
            );
        }

        private static ScoreTarget ToDomain(ScoreTargetEntity entity)
        {
            return new ScoreTarget(
                entity.Id,
                
                entity.RoundTargetId,
                
                entity.ScoreValue,
                entity.Golds,
                entity.Hits
            );
        }

        private static void PopulateEntity(RoundEntity entity, ScoreInputModel score)
        {
            throw new NotImplementedException();
        }
    }
}