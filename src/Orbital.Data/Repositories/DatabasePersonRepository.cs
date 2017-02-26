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
    public class DatabasePersonRepository : IPersonRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public DatabasePersonRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public IReadOnlyCollection<Person> GetAll()
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var people = connection.GetAll<PersonEntity>();

                return people.Select(PersonMapper.ToDomain).ToList();
            }
        }

        public IReadOnlyCollection<Person> GetAllByClubId(int clubId)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var people = connection.Query<PersonEntity>(@"SELECT * FROM Person WHERE ""ClubId"" = @ClubId", new { ClubId = clubId });

                return people.Select(PersonMapper.ToDomain).ToList();
            }
        }

        public Person GetById(int id)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var person = connection.Get<PersonEntity>(id);

                return person?.ToDomain();
            }
        }

        public Person Create(Person person)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var entity = person.ToEntity();

                entity.Id = (int)connection.Insert(entity);

                return entity.ToDomain();
            }
        }

        public Person Update(Person person)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var entity = person.ToEntity();

                connection.Update(entity);

                return entity.ToDomain();
            }
        }

        public bool Delete(Person person)
        {
            using (var connection = _dbFactory.GetConnection())
            {
                var entity = person.ToEntity();

                return connection.Delete(entity);
            }
        }
    }
}