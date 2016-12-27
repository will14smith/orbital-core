using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Orbital.Data
{
    static class DataReaderExtensions
    {

        public static IReadOnlyCollection<T> ReadAll<T>(this IDataReader reader, Func<IDataRecord, T> mapToObjectFn)
        {
            var results = new List<T>();

            while (reader.Read())
            {
                results.Add(mapToObjectFn(reader));
            }

            return results;
        }

        public static T ReadOne<T>(this IDataReader reader, Func<IDataRecord, T> mapToObjectFn)
        {
            var results = reader.ReadAll(mapToObjectFn);

            return results.SingleOrDefault();
        }
    }
}
