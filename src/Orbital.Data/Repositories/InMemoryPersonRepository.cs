using System.Collections.Generic;
using System.Linq;
using Orbital.Models.Domain;
using Orbital.Models.Repositories;

namespace Orbital.Data.Repositories
{
    public class InMemoryPersonRepository : InMemoryRepository<Person>, IPersonRepository
    {
        private InMemoryPersonRepository(List<Person> data) : base(data) { }

        public static InMemoryPersonRepository New(params Person[] people)
        {
            return new InMemoryPersonRepository(people.ToList());
        }

        public IReadOnlyCollection<Person> GetAllByClubId(int clubId) => Data.Where(x => x.ClubId == clubId).ToList();

        protected override int GetId(Person item) => item.Id;
    }
}
