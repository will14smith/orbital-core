using System.Collections.Generic;
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
    public class DatabaseHandicapRepository : IHandicapRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public DatabaseHandicapRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public IReadOnlyCollection<Handicap> GetAll()
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var handicaps = connection.GetAll<HandicapEntity>();

                return handicaps.Select(HandicapMapper.ToDomain).ToList();
            }
        }

        public ILookup<HandicapIdentifier, Handicap> GetAllByPersonId(int personId)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var handicaps = connection.Query<HandicapEntity>(@"SELECT * FROM handicap WHERE ""PersonId"" = @PersonId", new { PersonId = personId });

                return handicaps.Select(HandicapMapper.ToDomain)
                    .ToLookup(x => x.Identifier);
            }
        }

        public IReadOnlyDictionary<HandicapIdentifier, Handicap> GetLatestByPersonId(int personId)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                // Inner query: select the latest handicap id per (person, identifier) group
                // Outer query: select the actual handicaps
                var handicaps = connection.Query<HandicapEntity>(@"
                    SELECT h1.*
                    FROM handicap as h1
                    INNER JOIN (
                        SELECT MAX(hi.""Id"") AS maxid
                        FROM handicap as hi 
                        GROUP BY hi.""PersonId"", hi.""Indoor"", hi.""Bowstyle""
                    ) as h2 ON h1.""Id"" = p2.maxid
                    WHERE h1.""PersonId"" = @PersonId
                ", new { PersonId = personId });

                return handicaps.Select(HandicapMapper.ToDomain)
                    .ToDictionary(x => x.Identifier);
            }
        }


        public Handicap GetById(int id)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var handicap = connection.Get<HandicapEntity>(id);

                return handicap?.ToDomain();
            }
        }

        public Handicap GetLatestByPersonId(HandicapIdentifier id, int personId)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var handicaps = connection.Query<HandicapEntity>(@"
                    SELECT * FROM handicap
                    WHERE ""PersonId"" = @PersonId
                    AND ""Indoor"" = @Indoor
                    AND ""Bowstyle"" = @Bowstyle
                    ORDER BY Id DESC
                ", new { PersonId = personId, Indoor = id.Indoor, Bowstyle = (int)id.Bowstyle });

                var handicap = handicaps.FirstOrDefault();
                return handicap.ToDomain();
            }
        }

        public Handicap GetByScoreId(int scoreId)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var handicaps = connection.Query<HandicapEntity>(@"SELECT * FROM handicap WHERE ""ScoreId"" = @ScoreId", new { ScoreId = scoreId });

                var handicap = handicaps.SingleOrDefault();
                return handicap.ToDomain();
            }
        }

        public Handicap Create(Handicap handicap)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var entity = handicap.ToEntity();

                entity.Id = (int)connection.Insert(entity);

                return entity.ToDomain();
            }
        }

        public Handicap Update(Handicap handicap)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var entity = handicap.ToEntity();

                connection.Update(entity);

                return entity.ToDomain();
            }
        }

        public bool Delete(Handicap handicap)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var entity = handicap.ToEntity();

                return connection.Delete(entity);
            }
        }
    }
}