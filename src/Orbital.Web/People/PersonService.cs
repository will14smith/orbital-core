using System.Collections.Generic;
using System.Linq;
using Orbital.Models.Domain;
using Orbital.Models.Repositories;

namespace Orbital.Web.People
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;

        public PersonService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public IReadOnlyCollection<PersonViewModel> GetAll()
        {
            return _personRepository.GetAll().Select(ToViewModel).ToList();
        }

        public IReadOnlyCollection<PersonViewModel> GetAllByClubId(int clubId)
        {
            return _personRepository.GetAllByClubId(clubId).Select(ToViewModel).ToList();
        }

        public PersonViewModel GetById(int id)
        {
            var person = _personRepository.GetById(id);

            return person == null ? null : ToViewModel(person);
        }

        public PersonViewModel Create(PersonInputModel input)
        {
            var person = FromInputModel(input);

            var result = _personRepository.Create(person);

            return ToViewModel(result);
        }

        public PersonViewModel Update(PersonInputModel input)
        {
            var person = FromInputModel(input);

            var result = _personRepository.Update(person);

            return ToViewModel(result);
        }

        public bool Delete(int id)
        {
            var person = _personRepository.GetById(id);

            return _personRepository.Delete(person);
        }

        private static PersonViewModel ToViewModel(Person person)
        {
            return new PersonViewModel
            {
                Id = person.Id,

                ClubId = person.ClubId,

                Name = person.Name,

                Gender = person.Gender,
                Bowstyle = person.Bowstyle,
                ArcheryGBNumber = person.ArcheryGBNumber,

                DateOfBirth = person.DateOfBirth,
                DateStartedArchery = person.DateStartedArchery
            };
        }

        private static Person FromInputModel(PersonInputModel person)
        {
            return new Person(
                person.Id,

                person.ClubId,

                person.Name,

                person.Gender,
                person.Bowstyle,
                person.ArcheryGBNumber,

                person.DateOfBirth,
                person.DateStartedArchery
                );
        }
    }
}
