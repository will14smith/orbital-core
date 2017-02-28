using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Dapper.Contrib.Extensions;
using Orbital.Data.Connections;
using Orbital.Data.Entities;
using Orbital.Data.Mapping;
using Orbital.Models.Domain;
using Orbital.Models.Repositories;

namespace Orbital.Data.Repositories
{
    public class DatabaseScoreRepository : IScoreRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public DatabaseScoreRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public IReadOnlyCollection<Score> GetAll()
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var results = connection.QueryMultiple(@"
                    SELECT * FROM score;
                    SELECT * FROM score_target
                ");

                return ReadAll(results);
            }
        }

        public IReadOnlyCollection<Score> GetAllByPersonId(int personId)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var results = connection.QueryMultiple(@"
                    SELECT * FROM score WHERE ""PersonId"" = @PersonId;
                    SELECT score_target.* FROM score_target INNER JOIN score ON score_target.""ScoreId"" = score.""Id"" WHERE score.""PersonId"" = @PersonId
                ", new { PersonId = personId });

                return ReadAll(results);
            }
        }

        public IReadOnlyCollection<Score> GetAllByRoundId(int roundId)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var results = connection.QueryMultiple(@"
                    SELECT * FROM score WHERE ""RoundId"" = @RoundId;
                    SELECT score_target.* FROM score_target INNER JOIN score ON score_target.""ScoreId"" = score.""Id"" WHERE score.""RoundId"" = @RoundId
                ", new { RoundId = roundId });

                return ReadAll(results);
            }
        }

        public IReadOnlyCollection<Score> GetAllByCompetitionId(int competitionId)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var results = connection.QueryMultiple(@"
                    SELECT * FROM score WHERE ""CompetitionId"" = @CompetitionId;
                    SELECT score_target.* FROM score_target INNER JOIN score ON score_target.""ScoreId"" = score.""Id"" WHERE score.""CompetitionId"" = @CompetitionId
                ", new { CompetitionId = competitionId });

                return ReadAll(results);
            }
        }

        private static IReadOnlyCollection<Score> ReadAll(SqlMapper.GridReader results)
        {
            var scores = results.Read<ScoreEntity>();
            var scoreTargets = results.Read<ScoreTargetEntity>()
                .GroupBy(x => x.ScoreId)
                .ToDictionary(x => x.Key, x => (IEnumerable<ScoreTarget>)x.Select(rt => rt.ToDomain()).ToList());

            return scores.Select(x => x.ToDomain(scoreTargets.SafeGet(x.Id, () => new ScoreTarget[0]))).ToList();
        }

        public Score GetById(int id)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var results = connection.QueryMultiple(@"
                    SELECT * FROM score WHERE ""Id"" = @Id;
                    SELECT * FROM score_target WHERE ""ScoreId"" = @Id
                ", new { Id = id });

                var rounds = results.Read<ScoreEntity>().SingleOrDefault();
                var targets = results.Read<ScoreTargetEntity>();

                return rounds?.ToDomain(targets.Select(x => x.ToDomain()));
            }
        }

        public Score Create(Score score)
        {
            using (var connection = _dbFactory.GetConnection())
            using (var transaction = connection.BeginTransaction())
            {
                var entity = score.ToEntity();

                entity.Id = (int)connection.Insert(entity, transaction);

                var targets = score.Targets.Select(x => x.ToEntity(entity.Id)).ToList();

                if (!InsertTargets(connection, transaction, targets))
                {
                    transaction.Rollback();
                    throw new NotImplementedException("TODO");
                }

                transaction.Commit();

                return entity.ToDomain(targets.Select(x => x.ToDomain()));
            }
        }

        public Score Update(Score score)
        {
            using (var connection = _dbFactory.GetConnection())
            using (var transaction = connection.BeginTransaction())
            {
                var entity = score.ToEntity();

                if (!connection.Update(entity))
                {
                    transaction.Rollback();
                    throw new NotImplementedException("TODO");
                }

                connection.Execute(@"DELETE FROM score_target WHERE ""ScoreId"" = @Id", entity, transaction);

                var targets = score.Targets.Select(x => x.ToEntity(entity.Id)).ToList();
                if (!InsertTargets(connection, transaction, targets))
                {
                    transaction.Rollback();
                    throw new NotImplementedException("TODO");
                }

                transaction.Commit();

                return entity.ToDomain(targets.Select(x => x.ToDomain()));
            }
        }

        private static bool InsertTargets(IDbConnection connection, IDbTransaction transaction, IEnumerable<ScoreTargetEntity> scoreTargets)
        {
            var targets = scoreTargets.ToList();

            var count = connection.Insert(targets, transaction);

            return count == targets.Count;
        }


        public bool Delete(Score score)
        {
            using (var connection = _dbFactory.GetConnection())
            using (var transaction = connection.BeginTransaction())
            {
                var entity = score.ToEntity();

                connection.Execute(@"DELETE FROM score_target WHERE ""ScoreId"" = @Id", entity, transaction);

                if (!connection.Delete(entity, transaction))
                {
                    // TODO explicitly abort transaction?
                    return false;
                }

                transaction.Commit();

                return true;
            }
        }

    }
}