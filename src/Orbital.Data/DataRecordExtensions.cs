using System;
using System.Data;
using System.Reflection;

namespace Orbital.Data
{
    static class DataRecordExtensions
    {
        public static T GetValue<T>(this IDataRecord row, string fieldName)
        {
            int ordinal = row.GetOrdinal(fieldName);
            return row.GetValue<T>(ordinal);
        }

        public static T GetValue<T>(this IDataRecord row, int ordinal)
        {
            var value = row.GetValue(ordinal);
            if (value is DBNull)
            {
                return default(T);
            }

            var type = typeof(T);

            if (type.GetTypeInfo().IsEnum)
                return (T)Enum.ToObject(type, value);

            var nullableUnderlying = Nullable.GetUnderlyingType(type);
            if (nullableUnderlying != null && nullableUnderlying.GetTypeInfo().IsEnum)
                return (T)Enum.ToObject(nullableUnderlying, value);
            if (nullableUnderlying != null)
                return (T)Convert.ChangeType(value, nullableUnderlying);

            return (T)Convert.ChangeType(value, type);

        }
    }
}
