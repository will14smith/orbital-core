using System.Data;

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
            return (T)row.GetValue(ordinal);
        }

        public static T GetValueOrDefault<T>(this IDataRecord row, string fieldName)
        {
            int ordinal = row.GetOrdinal(fieldName);
            return row.GetValueOrDefault<T>(ordinal);
        }

        public static T GetValueOrDefault<T>(this IDataRecord row, int ordinal)
        {
            return (T)(row.IsDBNull(ordinal) ? default(T) : row.GetValue(ordinal));
        }
    }
}
