using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
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

                var rounds = results.Read<RoundEntity>();
                var roundTargets = results.Read<RoundTargetEntity>()
                    .GroupBy(x => x.RoundId)
                    .ToDictionary(x => x.Key, x => (IEnumerable<RoundTarget>)x.Select(rt => rt.ToDomain()).ToList());

                return rounds.Select(x => x.ToDomain(roundTargets.SafeGet(x.Id, () => new RoundTarget[0]))).ToList();
            }
        }

        public IReadOnlyCollection<Round> GetAllVariantsById(int parentRoundId)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var results = connection.QueryMultiple(@"
                    SELECT * FROM round WHERE VariantOfId = @ParentRoundId;
                    SELECT round_target.* FROM round_target INNER JOIN round ON round_target.RoundId = round.Id WHERE round.VariantOfId = @ParentRoundId
                ", new { ParentRoundId = parentRoundId });

                var rounds = results.Read<RoundEntity>();
                var roundTargets = results.Read<RoundTargetEntity>()
                    .GroupBy(x => x.RoundId)
                    .ToDictionary(x => x.Key, x => (IEnumerable<RoundTarget>)x.Select(rt => rt.ToDomain()).ToList());

                return rounds.Select(x => x.ToDomain(roundTargets.SafeGet(x.Id, () => new RoundTarget[0]))).ToList();
            }
        }

        public Round GetById(int id)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var results = connection.QueryMultiple(@"
                    SELECT * FROM round WHERE Id = @Id;
                    SELECT * FROM round_target WHERE RoundId = @Id
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

                entity.Id = connection.ExecuteScalar<int>(@"
    INSERT INTO round (VariantOfId, Category, Name, Indoor) 
    VALUES (@VariantOfId, @Category, @Name, @Indoor) RETURNING Id", entity);

                var targets = round.Targets.Select(x => x.ToEntity(entity.Id)).ToList();

                if (!InsertTargets(connection, targets))
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

                connection.Execute("UPDATE round SET VariantOfId = @VariantOfId, Category = @Category, Name = @Name, Indoor = @Indoor WHERE Id = @Id", entity);
                connection.Execute("DELETE FROM round_target WHERE RoundId = @Id", entity);

                var targets = round.Targets.Select(x => x.ToEntity(entity.Id)).ToList();

                if (!InsertTargets(connection, targets))
                {
                    transaction.Rollback();
                    throw new NotImplementedException("TODO");
                }

                transaction.Commit();

                return entity.ToDomain(targets.Select(x => x.ToDomain()));
            }
        }

        private static bool InsertTargets(IDbConnection connection, IEnumerable<RoundTargetEntity> roundTargets)
        {
            var targets = roundTargets.ToList();

            var count = connection.Execute(@"
    INSERT INTO round_target(RoundId, ScoringType, DistanceValue, DistanceUnit, FaceSizeValue, FaceSizeUnit, ArrowCount) 
    VALUES(@RoundId, @ScoringType, @DistanceValue, @DistanceUnit, @FaceSizeValue, @FaceSizeUnit, @ArrowCount) RETURNING Id", targets);

            return count == targets.Count;
        }

        public bool Delete(Round round)
        {
            using (var connection = _dbFactory.GetConnection())
            using (var transaction = connection.BeginTransaction())
            {
                var entity = round.ToEntity();

                connection.Execute("DELETE FROM round_target WHERE RoundId = @Id", entity);
                var rowsChanged = connection.Execute("DELETE FROM round WHERE Id = @Id", entity);

                if (rowsChanged != 1)
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
