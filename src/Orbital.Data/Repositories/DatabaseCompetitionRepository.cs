using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NpgsqlTypes;
using Orbital.Data.Connections;
using Orbital.Models.Domain;
using Orbital.Models.Repositories;

namespace Orbital.Data.Repositories
{
    public class DatabaseCompetitionRepository : ICompetitionRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public DatabaseCompetitionRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public IReadOnlyCollection<Competition> GetAll()
        {
            IReadOnlyCollection<Competition> competitions;

            using (var connection = _dbFactory.GetConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT {SelectFields} FROM competition";

                using (var reader = command.ExecuteReader())
                {
                    competitions = reader.ReadAll(MapToCompetition);
                }
            }

            var roundIds = GetRoundIdsByCompetitionIds(competitions.Select(x => x.Id).ToList());

            return competitions
                .Select(x => new Competition(x.Id, x, roundIds.SafeGet(x.Id, _ => new int[0])))
                .ToList();
        }

        public Competition GetById(int id)
        {
            Competition competition;

            using (var connection = _dbFactory.GetConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT {SelectFields} FROM competition WHERE Id = @Id";
                command.Parameters.Add(command.CreateParameter("Id", id, DbType.Int32));

                using (var reader = command.ExecuteReader())
                {
                    competition = reader.ReadOne(MapToCompetition);
                }
            }

            if (competition == null)
            {
                return null;
            }

            var roundIds = GetRoundIdsByCompetitionIds(new[] { competition.Id });

            return new Competition(competition.Id, competition, roundIds.SafeGet(competition.Id, _ => new int[0]));
        }


        private IReadOnlyDictionary<int, IReadOnlyCollection<int>> GetRoundIdsByCompetitionIds(IReadOnlyCollection<int> competitionIds)
        {
            using (var connection = _dbFactory.GetConnection())
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT CompetitionId, RoundId FROM competition_round WHERE CompetitionId = ANY(@CompetitionIds)";
                command.Parameters.Add(command.CreateParameter("CompetitionIds", competitionIds.ToArray(), NpgsqlDbType.Array | NpgsqlDbType.Integer));

                using (var reader = command.ExecuteReader())
                {
                    return reader.ReadAll(MapToCompetitionRound)
                        .GroupBy(x => x.CompetitionId)
                        .ToDictionary(x => x.Key, x => (IReadOnlyCollection<int>)x.Select(y => y.RoundId).ToList());
                }
            }
        }

        public Competition Create(Competition competition)
        {
            using (var connection = _dbFactory.GetConnection())
            using (var transaction = connection.BeginTransaction())
            {
                int competitionId;

                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        "INSERT INTO competition (Name, Start, \"End\")" +
                        " VALUES (@Name, @Start, @End) RETURNING Id";

                    command.Parameters.Add(command.CreateParameter("Name", competition.Name, DbType.String));
                    command.Parameters.Add(command.CreateParameter("Start", competition.Start, DbType.DateTime));
                    command.Parameters.Add(command.CreateParameter("End", competition.End, DbType.DateTime));

                    competitionId = (int)command.ExecuteScalar();
                }

                var roundIds = new List<int>();
                foreach (var round in competition.Rounds)
                {
                    if (!InsertRound(connection, competitionId, round))
                    {
                        transaction.Rollback();
                        throw new NotImplementedException("TODO");
                    }

                    roundIds.Add(round);
                }

                transaction.Commit();

                return new Competition(competitionId, competition, roundIds);
            }
        }

        public Competition Update(Competition competition)
        {
            using (var connection = _dbFactory.GetConnection())
            using (var transaction = connection.BeginTransaction())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "UPDATE competition SET" +
                        " Name = @Name, " +
                        " Start = @Start, " +
                        " \"End\" = @End " +
                        " WHERE Id = @Id";

                    command.Parameters.Add(command.CreateParameter("Id", competition.Id, DbType.Int32));
                    command.Parameters.Add(command.CreateParameter("Name", competition.Name, DbType.String));
                    command.Parameters.Add(command.CreateParameter("Start", competition.Start, DbType.DateTime));
                    command.Parameters.Add(command.CreateParameter("End", competition.End, DbType.DateTime));

                    command.ExecuteNonQuery();
                }

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM competition_round WHERE CompetitionId = @CompetitionId";
                    command.Parameters.Add(command.CreateParameter("CompetitionId", competition.Id, DbType.Int32));
                    command.ExecuteNonQuery();
                }

                var roundIds = new List<int>();
                foreach (var round in competition.Rounds)
                {
                    if (!InsertRound(connection, competition.Id, round))
                    {
                        transaction.Rollback();
                        throw new NotImplementedException("TODO");
                    }

                    roundIds.Add(round);
                }

                transaction.Commit();

                return new Competition(competition.Id, competition, roundIds);
            }
        }

        private static bool InsertRound(IDbConnection connection, int competitionId, int roundId)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "INSERT INTO competition_round (CompetitionId, RoundId)" +
                    " VALUES (@CompetitionId, @RoundId)";

                command.Parameters.Add(command.CreateParameter("CompetitionId", competitionId, DbType.Int32));
                command.Parameters.Add(command.CreateParameter("RoundId", roundId, DbType.Int32));

                return command.ExecuteNonQuery() == 1;
            }
        }

        public bool Delete(Competition competition)
        {
            using (var connection = _dbFactory.GetConnection())
            using (var transaction = connection.BeginTransaction())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM competition_round WHERE CompetitionId = @CompetitionId";
                    command.Parameters.Add(command.CreateParameter("CompetitionId", competition.Id, DbType.Int32));
                    command.ExecuteNonQuery();
                }

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM competition WHERE Id = @Id";
                    command.Parameters.Add(command.CreateParameter("Id", competition.Id, DbType.Int32));
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

        private static readonly string SelectFields = "Id, Name, Start, \"End\"";
        private Competition MapToCompetition(IDataRecord record)
        {
            var id = record.GetValue<int>(0);

            var name = record.GetValue<string>(1);

            var start = record.GetValue<DateTime>(2);
            var end = record.GetValue<DateTime>(3);

            return new Competition(id, name, start, end, new int[0]);
        }
        private CompetitionIdAndRoundId MapToCompetitionRound(IDataRecord record)
        {
            var competitionId = record.GetValue<int>(0);
            var roundId = record.GetValue<int>(1);

            return new CompetitionIdAndRoundId(competitionId, roundId);
        }

        private class CompetitionIdAndRoundId
        {
            public CompetitionIdAndRoundId(int competitionId, int roundId)
            {
                CompetitionId = competitionId;
                RoundId = roundId;
            }

            public int CompetitionId { get; }
            public int RoundId { get; }
        }
    }
}
