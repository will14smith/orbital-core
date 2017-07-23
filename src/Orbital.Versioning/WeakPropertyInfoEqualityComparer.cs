using System.Collections.Generic;
using System.Reflection;

namespace Orbital.Versioning
{
    internal class WeakPropertyInfoEqualityComparer : IEqualityComparer<PropertyInfo>
    {
        public bool Equals(PropertyInfo x, PropertyInfo y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return ReferenceEquals(y, null);

            return x.Name == y.Name
                   && x.PropertyType.FullName == y.PropertyType.FullName;
        }

        public int GetHashCode(PropertyInfo obj)
        {
            return (obj.Name?.GetHashCode() ?? 0)
                   ^ obj.PropertyType.FullName.GetHashCode();
        }
    }
}