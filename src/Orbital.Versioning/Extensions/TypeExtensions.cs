using System;
using System.Reflection;

namespace Orbital.Versioning
{
    internal static class TypeExtensions
    {
        public static ConstructorInfo GetConstructor(this Type type, params Type[] parameterTypes)
        {
            return type.GetTypeInfo().GetConstructor(parameterTypes);
        }
    }
}
