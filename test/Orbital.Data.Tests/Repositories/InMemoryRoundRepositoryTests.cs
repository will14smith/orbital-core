using Orbital.Data.Repositories;
using Orbital.Models.Domain;
using Xunit;

namespace Orbital.Data.Tests.Repositories
{
    public class InMemoryRoundRepositoryTests
    {
        [Fact]
        public void TestGetAllVariantsById()
        {
            var rounds = new[]
            {
                new Round(1, 3, "Category1", "Name1", true, new RoundTarget[0]),
                new Round(2, null, "Category2", "Name2", false, new RoundTarget[0])
            };

            var repo = InMemoryRoundRepository.New(rounds);

            var result1 = repo.GetAllVariantsById(1);
            Assert.Empty(result1);

            var result2 = repo.GetAllVariantsById(3);
            Assert.Equal(new[] { rounds[0] }, result2);
        }

        [Fact]
        public void TestGetAllVariantsByIdEmpty()
        {
            var repo = InMemoryRoundRepository.New();

            var result = repo.GetAllVariantsById(1);
            Assert.Empty(result);
        }
    }
}

