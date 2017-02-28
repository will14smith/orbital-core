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
    public class DatabaseRoundRepository : IRoundRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public DatabaseRoundRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public IReadOnlyCollection<Round> GetAll()
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var results = connection.QueryMultiple(@"
                    SELECT * FROM round;
                    SELECT * FROM round_target
                ");

                return ReadAll(results);
            }
        }

        public IReadOnlyCollection<Round> GetAllVariantsById(int parentRoundId)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var results = connection.QueryMultiple(@"
                    SELECT * FROM round WHERE ""VariantOfId"" = @ParentRoundId;
                    SELECT round_target.* FROM round_target INNER JOIN round ON round_target.""RoundId"" = round.""Id"" WHERE round.""VariantOfId"" = @ParentRoundId
                ", new { ParentRoundId = parentRoundId });

                return ReadAll(results);
            }
        }

        private static IReadOnlyCollection<Round> ReadAll(SqlMapper.GridReader results)
        {
            var rounds = results.Read<RoundEntity>();
            var roundTargets = results.Read<RoundTargetEntity>()
                .GroupBy(x => x.RoundId)
                .ToDictionary(x => x.Key, x => (IEnumerable<RoundTarget>)x.Select(rt => rt.ToDomain()).ToList());

            return rounds.Select(x => x.ToDomain(roundTargets.SafeGet(x.Id, () => new RoundTarget[0]))).ToList();
        }

        public Round GetById(int id)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var results = connection.QueryMultiple(@"
                    SELECT * FROM round WHERE ""Id"" = @Id;
                    SELECT * FROM round_target WHERE ""RoundId"" = @Id
                ", new { Id = id });

                var rounds = results.Read<RoundEntity>().SingleOrDefault();
                var targets = results.Read<RoundTargetEntity>();

                return rounds?.ToDomain(targets.Select(x => x.ToDomain()));
            }
        }

        public Round Create(Round round)
        {
            using (var connection = _dbFactory.GetConnection())
            using (var transaction = connection.BeginTransaction())
            {
                var entity = round.ToEntity();

                entity.Id = (int)connection.Insert(entity, transaction);

                var targets = round.Targets.Select(x => x.ToEntity(entity.Id)).ToList();

                if (!InsertTargets(connection, transaction, targets))
                {
                    transaction.Rollback();
                    throw new NotImplementedException("TODO");
                }

                transaction.Commit();

                return entity.ToDomain(targets.Select(x => x.ToDomain()));
            }
        }

        public Round Update(Round round)
        {
            using (var connection = _dbFactory.GetConnection())
            using (var transaction = connection.BeginTransaction())
            {
                var entity = round.ToEntity();

                if (!connection.Update(entity))
                {
                    transaction.Rollback();
                    throw new NotImplementedException("TODO");
                }

                connection.Execute(@"DELETE FROM round_target WHERE ""RoundId"" = @Id", entity, transaction);

                var targets = round.Targets.Select(x => x.ToEntity(entity.Id)).ToList();
                if (!InsertTargets(connection, transaction, targets))
                {
                    transaction.Rollback();
                    throw new NotImplementedException("TODO");
                }

                transaction.Commit();

                return entity.ToDomain(targets.Select(x => x.ToDomain()));
            }
        }

        private static bool InsertTargets(IDbConnection connection, IDbTransaction transaction, IEnumerable<RoundTargetEntity> roundTargets)
        {
            var targets = roundTargets.ToList();

            var count = connection.Insert(targets, transaction);

            return count == targets.Count;
        }

        public bool Delete(Round round)
        {
            using (var connection = _dbFactory.GetConnection())
            using (var transaction = connection.BeginTransaction())
            {
                var entity = round.ToEntity();

                connection.Execute(@"DELETE FROM round_target WHERE ""RoundId"" = @Id", entity, transaction);

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
