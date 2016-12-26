using System;
using System.Linq;
using GraphQL.Types;
using Moq;
using Xunit;

namespace Orbital.Schema.Tests
{
  public class EnumTypeTests
  {
    [Fact]
    public void TestCreatingWithAnEnum()
    {
      var type = new EnumType<TestEnum>();

      Assert.Equal("TestEnumEnum", type.Name);

      var values = type.Values.ToList();

      Assert.Equal(2, values.Count);

      Assert.Equal(nameof(TestEnum.Value1), values[0].Name);
      Assert.Equal(TestEnum.Value1, values[0].Value);

      Assert.Equal(nameof(TestEnum.Value2), values[1].Name);
      Assert.Equal(TestEnum.Value2, values[1].Value);
    }
    [Fact]
    public void TestCreatingWithClass()
    {
      Assert.Throws<ArgumentException>(() =>
      {
        new EnumType<TestClass>();
      });
    }

    private enum TestEnum
    {
      Value1,
      Value2 = 4,
    }

    private class TestClass { }
  }
}
