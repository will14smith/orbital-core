using Orbital.Data.Repositories;
using Orbital.Models.Domain;
using Xunit;

namespace Orbital.Data.Tests.Repositories
{
    public class InMemoryPersonRepositoryTests
    {
        [Fact]
        public void TestGetAll()
        {
            var people = new[]
            {
                new Person(1, 0, "Person1", Gender.Male),
                new Person(2, 0, "Person2", Gender.Male)
            };

            var repo = InMemoryPersonRepository.New(people);

            var result = repo.GetAll();
            Assert.Equal(people, result);
        }

        [Fact]
        public void TestGetAllEmpty()
        {
            var repo = InMemoryPersonRepository.New();

            var result = repo.GetAll();
            Assert.Empty(result);
        }

        [Fact]
        public void TestGetById()
        {
            var person = new Person(1, 0, "Person1", Gender.Male);
            var repo = InMemoryPersonRepository.New(person);

            var result = repo.GetById(1);
            Assert.Equal(person, result);
        }
        [Fact]
        public void TestGetById_Missing()
        {
            var person = new Person(1, 0, "Person1", Gender.Male);
            var repo = InMemoryPersonRepository.New(person);

            var result = repo.GetById(2);
            Assert.Null(result);
        }

        [Fact]
        public void TestCreate()
        {
            var person = new Person(1, 0, "Person1", Gender.Male);
            var repo = InMemoryPersonRepository.New();

            var createResult = repo.Create(person);
            Assert.Equal(person, createResult);

            var result = repo.GetById(1);
            Assert.Equal(person, result);
        }

        [Fact]
        public void TestUpdate()
        {
            var person = new Person(1, 0, "Person1", Gender.Male);
            var repo = InMemoryPersonRepository.New(person);

            var newPerson = new Person(1, 0, "Person1-Updated", Gender.Female);

            var updateResult = repo.Update(newPerson);
            Assert.Equal(newPerson, updateResult);

            var result = repo.GetById(1);
            Assert.Equal(newPerson, result);
        }

        [Fact]
        public void TestRemove()
        {
            var person = new Person(1, 0, "Person1", Gender.Male);
            var repo = InMemoryPersonRepository.New(person);

            var deleteResult = repo.Delete(person);
            Assert.True(deleteResult);

            var result = repo.GetAll();
            Assert.Empty(result);
        }
    }
}
