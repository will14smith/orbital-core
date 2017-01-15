using System.Collections.Generic;
using System.Data;
using System.Linq;
using NpgsqlTypes;
using Orbital.Data.Connections;
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
            IReadOnlyCollection<Round> rounds;

            using (var connection = _dbFactory.GetConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT {RoundSelectFields} FROM Round";

                using (var reader = command.ExecuteReader())
                {
                    rounds = reader.ReadAll(MapToRound);
                }
            }

            var targets = GetTargetsByRoundIds(rounds.Select(x => x.Id).ToList());

            return rounds
                .Select(x => new Round(x.Id, x, targets.SafeGet(x.Id, _ => new RoundTarget[0])))
                .ToList();
        }

        public IReadOnlyCollection<Round> GetAllVariantsById(int parentRoundId)
        {
            IReadOnlyCollection<Round> rounds;

            using (var connection = _dbFactory.GetConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT {RoundSelectFields} FROM Round WHERE VariantOfId = @ParentRoundId";
                command.Parameters.Add(command.CreateParameter("ParentRoundId", parentRoundId, DbType.Int32));

                using (var reader = command.ExecuteReader())
                {
                    rounds = reader.ReadAll(MapToRound);
                }
            }

            var targets = GetTargetsByRoundIds(rounds.Select(x => x.Id).ToList());

            return rounds
                .Select(x => new Round(x.Id, x, targets.SafeGet(x.Id, _ => new RoundTarget[0])))
                .ToList();
        }

        public Round GetById(int id)
        {
            Round round;

            using (var connection = _dbFactory.GetConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT {RoundSelectFields} FROM Round WHERE Id = @Id";
                command.Parameters.Add(command.CreateParameter("Id", id, DbType.Int32));

                using (var reader = command.ExecuteReader())
                {
                    round = reader.ReadOne(MapToRound);
                }
            }

            if (round == null)
            {
                return null;
            }

            var targets = GetTargetsByRoundIds(new[] { round.Id });

            return new Round(round.Id, round, targets.SafeGet(round.Id, _ => new RoundTarget[0]));
        }


        private IReadOnlyDictionary<int, IReadOnlyCollection<RoundTarget>> GetTargetsByRoundIds(IReadOnlyCollection<int> roundIds)
        {
            using (var connection = _dbFactory.GetConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT {Round_TargetSelectFields} FROM Round_Target WHERE RoundId = ANY(@RoundIds)";
                command.Parameters.Add(command.CreateParameter("RoundIds", roundIds.ToArray(), NpgsqlDbType.Array | NpgsqlDbType.Integer));

                using (var reader = command.ExecuteReader())
                {
                    return reader.ReadAll(MapToRound_Target)
                        .GroupBy(x => x.RoundId)
                        .ToDictionary(x => x.Key, x => (IReadOnlyCollection<RoundTarget>)x.Select(y => y.RoundTarget).ToList());
                }
            }
        }

        public Round Create(Round round)
        {
            using (var connection = _dbFactory.GetConnection())
            using (var transaction = connection.BeginTransaction())
            {
                int roundId;

                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        "INSERT INTO Round (VariantOfId, Category, Name, Indoor)" +
                        " VALUES (@VariantOfId, @Category, @Name, @Indoor) RETURNING Id";

                    command.Parameters.Add(command.CreateParameter("VariantOfId", round.VariantOfId, DbType.Int32));
                    command.Parameters.Add(command.CreateParameter("Category", round.Category, DbType.String));
                    command.Parameters.Add(command.CreateParameter("Name", round.Name, DbType.String));
                    command.Parameters.Add(command.CreateParameter("Indoor", round.Indoor, DbType.Boolean));

                    roundId = (int)command.ExecuteScalar();
                }

                var targetsWithId = new List<RoundTarget>();

                foreach (var target in round.Targets)
                {
                    targetsWithId.Add(InsertTarget(connection, roundId, target));
                }

                transaction.Commit();

                return new Round(roundId, round, targetsWithId);
            }
        }

        public Round Update(Round round)
        {
            using (var connection = _dbFactory.GetConnection())
            using (var transaction = connection.BeginTransaction())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE Round SET" +
                        " VariantOfId = @VariantOfId, " +
                        " Category = @Category," +
                        " Name = @Name, " +
                        " Indoor = @Indoor" +
                        " WHERE Id = @Id";

                    command.Parameters.Add(command.CreateParameter("Id", round.Id, DbType.Int32));
                    command.Parameters.Add(command.CreateParameter("VariantOfId", round.VariantOfId, DbType.Int32));
                    command.Parameters.Add(command.CreateParameter("Category", round.Category, DbType.String));
                    command.Parameters.Add(command.CreateParameter("Name", round.Name, DbType.String));
                    command.Parameters.Add(command.CreateParameter("Indoor", round.Indoor, DbType.Boolean));

                    command.ExecuteNonQuery();
                }

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM Round_Target WHERE RoundId = @RoundId";
                    command.Parameters.Add(command.CreateParameter("RoundId", round.Id, DbType.Int32));
                    command.ExecuteNonQuery();
                }

                var targetsWithId = new List<RoundTarget>();
                foreach (var target in round.Targets)
                {
                    targetsWithId.Add(InsertTarget(connection, round.Id, target));
                }

                transaction.Commit();

                return new Round(round.Id, round, targetsWithId);
            }
        }

        private static RoundTarget InsertTarget(IDbConnection connection, int roundId, RoundTarget target)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "INSERT INTO Round_Target (RoundId, ScoringType, DistanceValue, DistanceUnit, FaceSizeValue, FaceSizeUnit, ArrowCount)" +
                    " VALUES (@RoundId, @ScoringType, @DistanceValue, @DistanceUnit, @FaceSizeValue, @FaceSizeUnit, @ArrowCount) RETURNING Id";

                command.Parameters.Add(command.CreateParameter("RoundId", roundId, DbType.Int32));
                command.Parameters.Add(command.CreateParameter("ScoringType", (int)target.ScoringType, DbType.Int32));
                command.Parameters.Add(command.CreateParameter("DistanceValue", target.Distance.Value, DbType.Decimal));
                command.Parameters.Add(command.CreateParameter("DistanceUnit", (int)target.Distance.Unit, DbType.Int32));
                command.Parameters.Add(command.CreateParameter("FaceSizeValue", target.FaceSize.Value, DbType.Decimal));
                command.Parameters.Add(command.CreateParameter("FaceSizeUnit", (int)target.FaceSize.Unit, DbType.Int32));
                command.Parameters.Add(command.CreateParameter("ArrowCount", target.ArrowCount, DbType.Int32));

                var insertId = (int)command.ExecuteScalar();
                return new RoundTarget(insertId, target);
            }
        }

        public bool Delete(Round round)
        {
            using (var connection = _dbFactory.GetConnection())
            using (var transaction = connection.BeginTransaction())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM Round_Target WHERE RoundId = @RoundId";
                    command.Parameters.Add(command.CreateParameter("RoundId", round.Id, DbType.Int32));
                    command.ExecuteNonQuery();
                }

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM Round WHERE Id = @Id";
                    command.Parameters.Add(command.CreateParameter("Id", round.Id, DbType.Int32));
                    var rowsChanged = command.ExecuteNonQuery();

                    if (rowsChanged != 1)
                    {
                        return false;
                    }
                }

                transaction.Commit();

                return true;
            }
        }


        private static readonly string RoundSelectFields = "Id, VariantOfId, Category, Name, Indoor";
        private Round MapToRound(IDataRecord record)
        {
            var id = record.GetValue<int>(0);

            var variantOfId = record.GetValue<int?>(1);

            var category = record.GetValue<string>(2);
            var name = record.GetValue<string>(3);
            var indoor = record.GetValue<bool>(4);

            return new Round(id, variantOfId, category, name, indoor, new RoundTarget[0]);
        }
        private static readonly string Round_TargetSelectFields = "Id, RoundId, ScoringType, DistanceValue, DistanceUnit, FaceSizeValue, FaceSizeUnit, ArrowCount";
        private RoundTargetAndRoundId MapToRound_Target(IDataRecord record)
        {
            var id = record.GetValue<int>(0);

            var roundId = record.GetValue<int>(1);

            var scoringType = record.GetValue<ScoringType>(2);

            var distanceValue = record.GetValue<decimal>(3);
            var distanceUnit = record.GetValue<LengthUnit>(4);
            var faceSizeValue = record.GetValue<decimal>(5);
            var faceSizeUnit = record.GetValue<LengthUnit>(6);

            var arrowCount = record.GetValue<int>(7);

            var distance = new Length(distanceValue, distanceUnit);
            var faceSize = new Length(faceSizeValue, faceSizeUnit);

            var target = new RoundTarget(id, scoringType, distance, faceSize, arrowCount);

            return new RoundTargetAndRoundId(roundId, target);
        }

        private class RoundTargetAndRoundId
        {
            public RoundTargetAndRoundId(int roundId, RoundTarget roundTarget)
            {
                RoundId = roundId;
                RoundTarget = roundTarget;
            }

            public int RoundId { get; }
            public RoundTarget RoundTarget { get; }
        }
    }
}
