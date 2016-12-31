using System.Collections.Generic;
using System.Linq;
using Orbital.Models.Domain;
using Orbital.Models.Repositories;

namespace Orbital.Data.Repositories
{
    public class InMemoryPersonRepository : IPersonRepository
    {
        private List<Person> _people;

        public InMemoryPersonRepository()
        {
            _people = new List<Person>();
        }

        public static InMemoryPersonRepository New(params Person[] people)
        {
            return new InMemoryPersonRepository { _people = people.ToList() };
        }

        public IReadOnlyCollection<Person> GetAll() { return _people; }
        public IReadOnlyCollection<Person> GetAllByClubId(int clubId) { return _people.Where(x => x.ClubId == clubId).ToList(); }
        public Person GetById(int id) { return _people.FirstOrDefault(x => x.Id == id); }

        public Person Create(Person person) { _people.Add(person); return person; }
        public Person Update(Person person) { Delete(person); return Create(person); }
        public bool Delete(Person person) { return _people.Remove(GetById(person.Id)); }
    }
}
