using System;
using GraphQL.Types;
using Moq;
using Xunit;

namespace Orbital.Schema.Tests
{
  public class ContextExtensionsTests
  {
    [Fact]
    public void TestResolveService()
    {
      var service = "hello world";
      var userContext = new Mock<IUserContext>();
      userContext.Setup(x => x.ResolveService<string>()).Returns(service).Verifiable();

      var context = new ResolveFieldContext<object> { UserContext = userContext.Object };
      var result = ContextExtensions.ResolveService<object, string>(context);

      userContext.Verify();

      Assert.Equal(service, result);
    }
    [Fact]
    public void TestResolveService_InvalidContext()
    {
      var service = "hello world";

      var context = new ResolveFieldContext<object> { UserContext = service };
      Assert.Throws<InvalidCastException>(() =>
      {
        ContextExtensions.ResolveService<object, string>(context);
      });
    }
  }
}
