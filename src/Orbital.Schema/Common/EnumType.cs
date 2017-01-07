using System;
using System.Reflection;
using GraphQL.Types;

namespace Orbital.Schema.Common
{
  public class EnumType<TEnum> : EnumerationGraphType
  {
    public EnumType() : base()
    {
      var enumType = typeof(TEnum);
      if (!enumType.GetTypeInfo().IsEnum)
      {
        throw new ArgumentException(nameof(TEnum), "Expecting an enum type");
      }

      Name = enumType.Name + "Enum";

      foreach (var value in Enum.GetValues(enumType))
      {
        var name = Enum.GetName(enumType, value);
        AddValue(name, string.Format("{0} in {1}", name, enumType.Name), value);
      }
    }
  }
}