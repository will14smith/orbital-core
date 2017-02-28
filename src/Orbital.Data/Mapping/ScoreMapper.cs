using System.Collections.Generic;
using System.Linq;
using Orbital.Data.Entities;
using Orbital.Models.Domain;

namespace Orbital.Data.Mapping
{
    internal static class ScoreMapper
    {
        public static Score ToDomain(this ScoreEntity entity, IEnumerable<ScoreTarget> targets)
        {
            return new Score(entity.Id, entity.PersonId, entity.ClubId, entity.RoundId, entity.CompetitionId, (Bowstyle) entity.Bowstyle, entity.TotalScore, entity.TotalGolds, entity.TotalHits, entity.ShotAt, entity.EnteredAt, targets.ToList());
        }
        public static ScoreEntity ToEntity(this Score domain)
        {
            return new ScoreEntity
            {
                Id = domain.Id,

                PersonId = domain.PersonId,
                ClubId = domain.ClubId,
                RoundId = domain.RoundId,
                CompetitionId = domain.CompetitionId,

                Bowstyle = (int) domain.Bowstyle,

                TotalScore = domain.TotalScore,
                TotalGolds = domain.TotalGolds,
                TotalHits = domain.TotalHits,

                ShotAt = domain.ShotAt,
                EnteredAt = domain.EnteredAt,
            };
        }

        public static ScoreTarget ToDomain(this ScoreTargetEntity entity)
        {
            return new ScoreTarget(entity.Id, entity.Score, entity.Golds, entity.Hits);
        }
        public static ScoreTargetEntity ToEntity(this ScoreTarget domain, int scoreId)
        {
            return new ScoreTargetEntity
            {
                Id = domain.Id,

                ScoreId = scoreId,

                Score = domain.Score,
                Golds = domain.Golds,
                Hits = domain.Hits,
            };
        }
    }
}
