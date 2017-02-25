using Orbital.Data.Repositories;
using Orbital.Models.Domain;
using Xunit;

namespace Orbital.Data.Tests.Repositories
{
    public class InMemoryPersonRepositoryTests
    {
        [Fact]
        public void TestGetAllByClubId()
        {
            var people = new[]
            {
                new Person(1, 1, "Person1", Gender.Male),
                new Person(2, 1, "Person2", Gender.Male)
            };

            var repo = InMemoryPersonRepository.New(people);

            var result = repo.GetAllByClubId(1);
            Assert.Equal(people, result);
        }

        [Fact]
        public void TestGetAllByClubIdNoResults()
        {
            var people = new[]
            {
                new Person(1, 1, "Person1", Gender.Male),
            };

            var repo = InMemoryPersonRepository.New(people);

            var result = repo.GetAllByClubId(2);
            Assert.Empty(result);
        }

        [Fact]
        public void TestGetAllByClubIdEmpty()
        {
            var repo = InMemoryPersonRepository.New();

            var result = repo.GetAllByClubId(1);
            Assert.Empty(result);
        }
    }
}
