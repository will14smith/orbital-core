using System.Collections.Generic;
using System.Linq;
using Dapper;
using Orbital.Data.Connections;
using Orbital.Data.Entities;
using Orbital.Data.Mapping;
using Orbital.Models.Domain;
using Orbital.Models.Repositories;

namespace Orbital.Data.Repositories
{
    public class DatabaseClubRepository : IClubRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public DatabaseClubRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public IReadOnlyCollection<Club> GetAll()
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var clubs = connection.Query<ClubEntity>("SELECT * FROM Club");

                return clubs.Select(ClubMapper.ToDomain).ToList();
            }
        }

        public Club GetById(int id)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var club = connection.QuerySingleOrDefault<ClubEntity>("SELECT * FROM Club WHERE Id = @Id", new { Id = id });

                return club?.ToDomain();
            }
        }

        public Club Create(Club club)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var entity = club.ToEntity();

                entity.Id = connection.ExecuteScalar<int>("INSERT INTO Club (Name) VALUES (@Name) RETURNING Id", entity);

                return entity.ToDomain();
            }
        }

        public Club Update(Club club)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var entity = club.ToEntity();

                connection.Execute("UPDATE Club SET Name = @Name WHERE Id = @Id", entity);

                return entity.ToDomain();
            }
        }

        public bool Delete(Club club)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var entity = club.ToEntity();

                var rowsChanged = connection.Execute("DELETE FROM Club WHERE Id = @Id", entity);

                return rowsChanged == 1;
            }
        }
    }
}