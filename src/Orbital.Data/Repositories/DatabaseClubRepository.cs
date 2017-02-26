using System.Collections.Generic;
using System.Linq;
using Dapper.Contrib.Extensions;
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
                var clubs = connection.GetAll<ClubEntity>();

                return clubs.Select(ClubMapper.ToDomain).ToList();
            }
        }

        public Club GetById(int id)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var club = connection.Get<ClubEntity>(id);

                return club?.ToDomain();
            }
        }

        public Club Create(Club club)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var entity = club.ToEntity();

                entity.Id = (int) connection.Insert(entity);

                return entity.ToDomain();
            }
        }

        public Club Update(Club club)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var entity = club.ToEntity();

                connection.Update(entity);

                return entity.ToDomain();
            }
        }

        public bool Delete(Club club)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var entity = club.ToEntity();

                return connection.Delete(entity);
            }
        }
    }
}