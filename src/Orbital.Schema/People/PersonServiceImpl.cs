using System.Collections.Generic;
using Orbital.Models.Domain;
using Orbital.Models.Repositories;

namespace Orbital.Schema.People
{
    public class PersonServiceImpl : IPersonService
    {
        private readonly IPersonRepository _personRepository;

        public PersonServiceImpl(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public IReadOnlyCollection<Person> GetRoot()
        {
            return _personRepository.GetAll();
        }

        public IReadOnlyCollection<Person> GetByClub(Club club)
        {
            return _personRepository.GetAllByClubId(club.Id);
        }

        public Person Add(Person input)
        {
            return _personRepository.Create(input);
        }

        public Person Update(int id, Person input)
        {
            var person = new Person(id, input);

            return _personRepository.Update(person);
        }
    }
}