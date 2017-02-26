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
    public class DatabaseCompetitionRepository : ICompetitionRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public DatabaseCompetitionRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public IReadOnlyCollection<Competition> GetAll()
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var results = connection.QueryMultiple(@"
                    SELECT * FROM competition;
                    SELECT * FROM competition_round
                ");

                var competitions = results.Read<CompetitionEntity>();
                var competitionRounds = results.Read<CompetitionRoundEntity>()
                    .GroupBy(x => x.CompetitionId)
                    .ToDictionary(x => x.Key, x => (IEnumerable<int>)x.Select(cr => cr.RoundId).ToList());

                return competitions.Select(x => x.ToDomain(competitionRounds.SafeGet(x.Id, () => new int[0]))).ToList();
            }
        }

        public Competition GetById(int id)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var results = connection.QueryMultiple(@"
                    SELECT * FROM competition WHERE ""Id"" = @Id;
                    SELECT * FROM competition_round WHERE ""CompetitionId"" = @Id
                ", new { Id = id });

                var competition = results.Read<CompetitionEntity>().SingleOrDefault();
                var competitionRounds = results.Read<CompetitionRoundEntity>();

                return competition?.ToDomain(competitionRounds.Select(x => x.RoundId));
            }
        }

        public Competition Create(Competition competition)
        {
            using (var connection = _dbFactory.GetConnection())
            using (var transaction = connection.BeginTransaction())
            {
                var entity = competition.ToEntity();

                entity.Id = (int)connection.Insert(entity, transaction);

                if (!InsertRounds(connection, transaction, entity.Id, competition.Rounds))
                {
                    transaction.Rollback();
                    throw new NotImplementedException("TODO");
                }

                transaction.Commit();

                return entity.ToDomain(competition.Rounds);
            }
        }

        public Competition Update(Competition competition)
        {
            using (var connection = _dbFactory.GetConnection())
            using (var transaction = connection.BeginTransaction())
            {
                var entity = competition.ToEntity();

                if (!connection.Update(entity, transaction))
                {
                    transaction.Rollback();
                    throw new NotImplementedException("TODO");
                }

                connection.Execute(@"DELETE FROM competition_round WHERE ""CompetitionId"" = @Id", entity, transaction);
                if (!InsertRounds(connection, transaction, entity.Id, competition.Rounds))
                {
                    transaction.Rollback();
                    throw new NotImplementedException("TODO");
                }

                transaction.Commit();

                return entity.ToDomain(competition.Rounds);
            }
        }

        private static bool InsertRounds(IDbConnection connection, IDbTransaction transaction, int competitionId, IEnumerable<int> roundId)
        {
            var rounds = roundId.Select(x => new CompetitionRoundEntity { CompetitionId = competitionId, RoundId = x }).ToList();
            var count = connection.Insert(rounds, transaction);

            return count == rounds.Count;
        }

        public bool Delete(Competition competition)
        {
            using (var connection = _dbFactory.GetConnection())
            using (var transaction = connection.BeginTransaction())
            {
                var entity = competition.ToEntity();

                connection.Execute(@"DELETE FROM competition_round WHERE ""CompetitionId"" = @Id", entity, transaction);

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
