using Moq;
using Orbital.Data.Repositories;
using Orbital.Models.Domain;
using Orbital.Models.Repositories;
using Orbital.Schema.People;
using Xunit;

namespace Orbital.Schema.Tests.People
{
    public class PersonServiceImplTests
    {
        [Fact]
        public void TestGetRoot()
        {
            var person = new Person(1, 2, "PersonName", Gender.Male);
            var personRepository = InMemoryPersonRepository.New(person);

            var service = new PersonServiceImpl(personRepository);

            var result = service.GetRoot();
            Assert.Single(result, person);
        }

        [Fact]
        public void TestGetByClub()
        {
            var club = new Club(2, "ClubName");
            var person = new Person(1, 2, "PersonName", Gender.Male);
            var personRepository = InMemoryPersonRepository.New(person);

            var service = new PersonServiceImpl(personRepository);

            var result = service.GetByClub(club);
            Assert.Single(result, person);
        }

        [Fact]
        public void TestGetByClub_Different()
        {
            var club = new Club(1, "ClubName");
            var person = new Person(1, 2, "PersonName", Gender.Male);
            var personRepository = InMemoryPersonRepository.New(person);

            var service = new PersonServiceImpl(personRepository);

            var result = service.GetByClub(club);
            Assert.Empty(result);
        }


        [Fact]
        public void TestAdd()
        {
            var person = new Person(0, 2, "PersonName", Gender.Male);
            var personRepositoryMock = new Mock<IPersonRepository>();
            personRepositoryMock.Setup(x => x.Create(person)).Returns<Person>(x => new Person(1, x)).Verifiable();

            var service = new PersonServiceImpl(personRepositoryMock.Object);

            var result = service.Add(person);
            Assert.Equal(new Person(1, person), result);

            personRepositoryMock.Verify();
        }

        [Fact]
        public void TestUpdate()
        {
            var person = new Person(1, 2, "PersonName", Gender.Male);
            var personRepositoryMock = new Mock<IPersonRepository>();
            personRepositoryMock.Setup(x => x.Update(person)).Returns<Person>(x => x).Verifiable();

            var service = new PersonServiceImpl(personRepositoryMock.Object);

            var result = service.Update(person.Id, person);
            Assert.Equal(person, result);

            personRepositoryMock.Verify();
        }
    }
}