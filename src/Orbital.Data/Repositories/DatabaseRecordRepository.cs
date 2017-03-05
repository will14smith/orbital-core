using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Dapper.Contrib.Extensions;
using Orbital.Data.Connections;
using Orbital.Data.Mapping;
using Orbital.Models.Domain;
using Orbital.Models.Repositories;

namespace Orbital.Data.Repositories
{
    public class DatabaseRecordRepository : IRecordRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public DatabaseRecordRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public IReadOnlyCollection<Record> GetAll()
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var results = connection.QueryMultiple(@"
                    SELECT * FROM record;
                    SELECT * FROM record_club;
                    SELECT * FROM record_round;
                ");

                return ReadAll(results);
            }
        }

        public IReadOnlyCollection<Record> GetAllByClubId(int clubId)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var results = connection.QueryMultiple(@"
                        SELECT record.* FROM record 
                        INNER JOIN record_club
                            ON record.""Id"" = record_club.""RecordId""
                        WHERE record_club.""ClubId"" = @ClubId;

                        SELECT * FROM record_club WHERE ""ClubId"" = @ClubId;

                        SELECT record_round.* FROM record_round 
                        INNER JOIN record_club
                            ON record_round.""RecordId"" = record_club.""RecordId""
                        WHERE record_club.""ClubId"" = @ClubId;
                    ", new { ClubId = clubId });

                return ReadAll(results);
            }
        }

        public IReadOnlyCollection<Record> GetAllByRoundId(int roundId)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var results = connection.QueryMultiple(@"
                        SELECT record.* FROM record 
                        INNER JOIN record_round
                            ON record.""Id"" = record_round.""RecordId""
                        WHERE record_round.""RoundId"" = @RoundId;

                        SELECT record_club.* FROM record_club 
                        INNER JOIN record_round
                            ON record_club.""RecordId"" = record_round.""RecordId""
                        WHERE record_round.""RoundId"" = @RoundId;

                        SELECT * FROM record_round WHERE ""RoundId"" = @RoundId;
                    ", new { RoundId = roundId });

                return ReadAll(results);
            }
        }

        private static IReadOnlyCollection<Record> ReadAll(SqlMapper.GridReader results)
        {
            var records = results.Read<RecordEntity>();
            var clubs = results.Read<RecordClubEntity>().GroupBy(x => x.RecordId).ToDictionary(x => x.Key, x => (IEnumerable<RecordClub>)x.Select(rt => rt.ToDomain()).ToList());
            var rounds = results.Read<RecordRoundEntity>().GroupBy(x => x.RecordId).ToDictionary(x => x.Key, x => (IEnumerable<RecordRound>)x.Select(rt => rt.ToDomain()).ToList());

            return records.Select(x => x.ToDomain(
                clubs.SafeGet(x.Id, () => new RecordClub[0]),
                rounds.SafeGet(x.Id, () => new RecordRound[0]))
            ).ToList();
        }

        public Record GetById(int id)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var results = connection.QueryMultiple(@"
                    SELECT * FROM record WHERE ""Id"" = @Id;
                    SELECT * FROM record_club WHERE ""RecordId"" = @Id;
                    SELECT * FROM record_round WHERE ""RecordId"" = @Id;
                ", new { Id = id });

                var records = results.Read<RecordEntity>().SingleOrDefault();
                var clubs = results.Read<RecordClubEntity>();
                var rounds = results.Read<RecordRoundEntity>();

                return records?.ToDomain(clubs.Select(x => x.ToDomain()), rounds.Select(x => x.ToDomain()));
            }
        }

        public Record Create(Record record)
        {
            using (var connection = _dbFactory.GetConnection())
            using (var transaction = connection.BeginTransaction())
            {
                var entity = record.ToEntity();

                entity.Id = (int)connection.Insert(entity, transaction);

                var clubs = record.Clubs.Select(x => x.ToEntity(entity.Id)).ToList();
                if (!InsertClubs(connection, transaction, clubs))
                {
                    transaction.Rollback();
                    throw new NotImplementedException("TODO");
                }

                var rounds = record.Rounds.Select(x => x.ToEntity(entity.Id)).ToList();
                if (!InsertRounds(connection, transaction, rounds))
                {
                    transaction.Rollback();
                    throw new NotImplementedException("TODO");
                }

                transaction.Commit();

                return entity.ToDomain(clubs.Select(x => x.ToDomain()), rounds.Select(x => x.ToDomain()));
            }
        }

        public Record Update(Record record)
        {
            using (var connection = _dbFactory.GetConnection())
            using (var transaction = connection.BeginTransaction())
            {
                var entity = record.ToEntity();

                if (!connection.Update(entity))
                {
                    transaction.Rollback();
                    throw new NotImplementedException("TODO");
                }

                connection.Execute(@"DELETE FROM record_club WHERE ""RecordId"" = @Id", entity, transaction);
                connection.Execute(@"DELETE FROM record_round WHERE ""RecordId"" = @Id", entity, transaction);

                var clubs = record.Clubs.Select(x => x.ToEntity(entity.Id)).ToList();
                if (!InsertClubs(connection, transaction, clubs))
                {
                    transaction.Rollback();
                    throw new NotImplementedException("TODO");
                }

                var rounds = record.Rounds.Select(x => x.ToEntity(entity.Id)).ToList();
                if (!InsertRounds(connection, transaction, rounds))
                {
                    transaction.Rollback();
                    throw new NotImplementedException("TODO");
                }

                transaction.Commit();

                return entity.ToDomain(clubs.Select(x => x.ToDomain()), rounds.Select(x => x.ToDomain()));
            }
        }

        private static bool InsertClubs(IDbConnection connection, IDbTransaction transaction, IEnumerable<RecordClubEntity> clubs)
        {
            var list = clubs.ToList();
            var count = connection.Insert(list, transaction);
            return count == list.Count;
        }
        private static bool InsertRounds(IDbConnection connection, IDbTransaction transaction, IEnumerable<RecordRoundEntity> rounds)
        {
            var list = rounds.ToList();
            var count = connection.Insert(list, transaction);
            return count == list.Count;
        }

        public bool Delete(Record record)
        {
            using (var connection = _dbFactory.GetConnection())
            using (var transaction = connection.BeginTransaction())
            {
                var entity = record.ToEntity();

                connection.Execute(@"DELETE FROM record_club WHERE ""RecordId"" = @Id", entity, transaction);
                connection.Execute(@"DELETE FROM record_round WHERE ""RecordId"" = @Id", entity, transaction);

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