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
    public class DatabaseBadgeRepository : IBadgeRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public DatabaseBadgeRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public IReadOnlyCollection<Badge> GetAll()
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var badges = connection.GetAll<BadgeEntity>();

                return badges.Select(BadgeMapper.ToDomain).ToList();
            }
        }

        public Badge GetById(int id)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var badge = connection.Get<BadgeEntity>(id);

                return badge?.ToDomain();
            }
        }

        public Badge Create(Badge badge)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var entity = badge.ToEntity();

                entity.Id = (int) connection.Insert(entity);

                return entity.ToDomain();
            }
        }

        public Badge Update(Badge badge)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var entity = badge.ToEntity();

                connection.Update(entity);

                return entity.ToDomain();
            }
        }

        public bool Delete(Badge badge)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var entity = badge.ToEntity();

                return connection.Delete(entity);
            }
        }
    }
}