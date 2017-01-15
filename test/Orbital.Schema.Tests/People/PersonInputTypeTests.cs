using System.Linq;
using Orbital.Schema.People;
using Xunit;

namespace Orbital.Schema.Tests.People
{
    public class PersonInputTypeTests
    {
        [Fact]
        public void TestFields()
        {
            var type = new PersonInputType();

            Assert.Equal(
                new[] { "club", "name", "gender", "bowstyle", "archeryGBNumber", "dateOfBirth", "dateStartedArchery" }.OrderBy(x => x),
                type.Fields.Select(x => x.Name).OrderBy(x => x)
            );
        }
    }
}
