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
    public class DatabaseBadgeHolderRepository : IBadgeHolderRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public DatabaseBadgeHolderRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public IReadOnlyCollection<BadgeHolder> GetAll()
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var holders = connection.GetAll<BadgeHolderEntity>();

                return holders.Select(BadgeHolderMapper.ToDomain).ToList();
            }
        }
        
        public IReadOnlyCollection<BadgeHolder> GetAllByBadgeId(int badgeId)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var holders = connection.Query<BadgeHolderEntity>(@"SELECT * FROM badge_holder WHERE ""BadgeId"" = @BadgeId", new { BadgeId = badgeId });

                return holders.Select(BadgeHolderMapper.ToDomain).ToList();
            }
        }

        public IReadOnlyCollection<BadgeHolder> GetAllByPersonId(int personId)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var holders = connection.Query<BadgeHolderEntity>(@"SELECT * FROM badge_holder WHERE ""PersonId"" = @PersonId", new { PersonId = personId });

                return holders.Select(BadgeHolderMapper.ToDomain).ToList();
            }
        }

        public BadgeHolder GetById(int id)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var holder = connection.Get<BadgeHolderEntity>(id);

                return holder?.ToDomain();
            }
        }

        public BadgeHolder Create(BadgeHolder holder)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var entity = holder.ToEntity();

                entity.Id = (int)connection.Insert(entity);

                return entity.ToDomain();
            }
        }

        public BadgeHolder Update(BadgeHolder holder)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var entity = holder.ToEntity();

                connection.Update(entity);

                return entity.ToDomain();
            }
        }

        public bool Delete(BadgeHolder holder)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var entity = holder.ToEntity();

                return connection.Delete(entity);
            }
        }

    }
}