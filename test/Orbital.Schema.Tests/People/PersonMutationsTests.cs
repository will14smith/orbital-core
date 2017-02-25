using System.Collections.Generic;
using System.Linq;
using GraphQL.Types;
using Moq;
using Orbital.Schema.People;
using Xunit;
using Orbital.Models.Domain;
using Orbital.Models.Services;

namespace Orbital.Schema.Tests.People
{
    public class PersonMutationsTests
    {
        private readonly ObjectGraphType _mutations;

        public PersonMutationsTests()
        {
            _mutations = new ObjectGraphType();
            PersonMutations.AddToRoot(_mutations);
        }

        [Fact]
        public void TestAddToRoot()
        {
            Assert.Equal(
              new[] { "addPerson", "updatePerson" }.OrderBy(x => x),
              _mutations.Fields.Select(x => x.Name).OrderBy(x => x)
            );
        }

        [Fact]
        public void TestAddPersonResolve()
        {
            var person = new Person(0, 2, "PersonName", Gender.Male);
            var input = new Dictionary<string, object>
            {
                { "clubId", person.ClubId },
                { "name", person.Name },
                { "gender", person.Gender },
                { "bowstyle", person.Bowstyle },
                { "archeryGBNumber", person.ArcheryGBNumber },
                { "dateOfBirth", person.DateOfBirth },
                { "dateStartedArchery", person.DateStartedArchery }
            };

            var personServiceMock = new Mock<IPersonService>();
            personServiceMock.Setup(x => x.Add(It.IsAny<Person>())).Returns<Person>(x => new Person(1, x)).Verifiable();
            var userContext = Mock.Of<IUserContext>(x => x.ResolveService<IPersonService>() == personServiceMock.Object);

            var field = _mutations.Fields.First(x => x.Name == "addPerson");
            var result = field.Resolver.Resolve(new ResolveFieldContext
            {
                Arguments = new Dictionary<string, object> { { "person", input } },
                UserContext = userContext
            });

            Assert.Equal(new Person(1, person), result);
            personServiceMock.Verify();
        }

        [Fact]
        public void TestUpdatePersonResolve()
        {
            var person = new Person(1, 2, "PersonName", Gender.Male);
            var input = new Dictionary<string, object>
            {
                { "clubId", person.ClubId },
                { "name", person.Name },
                { "gender", person.Gender },
                { "bowstyle", person.Bowstyle },
                { "archeryGBNumber", person.ArcheryGBNumber },
                { "dateOfBirth", person.DateOfBirth },
                { "dateStartedArchery", person.DateStartedArchery }
            };

            var personServiceMock = new Mock<IPersonService>();
            personServiceMock.Setup(x => x.Update(person.Id, It.IsAny<Person>())).Returns<int, Person>((id, x) => new Person(id, x)).Verifiable();
            var userContext = Mock.Of<IUserContext>(x => x.ResolveService<IPersonService>() == personServiceMock.Object);

            var field = _mutations.Fields.First(x => x.Name == "updatePerson");
            var result = field.Resolver.Resolve(new ResolveFieldContext
            {
                Arguments = new Dictionary<string, object> { { "id", person.Id }, { "person", input } },
                UserContext = userContext
            });

            Assert.Equal(person, result);
            personServiceMock.Verify();
        }
    }
}
