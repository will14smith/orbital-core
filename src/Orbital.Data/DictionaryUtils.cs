using System;
using System.Collections.Generic;

namespace Orbital.Data
{
    internal static class DictionaryUtils
    {
        public static TValue SafeGet<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dict, TKey key, TValue defaultValue = default(TValue))
        {
            return dict.SafeGet(key, () => defaultValue);
        }

        public static TValue SafeGet<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dict, TKey key, Func<TValue> defaultValueFn)
        {
            return dict.SafeGet(key, _ => defaultValueFn());
        }
        public static TValue SafeGet<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dict, TKey key, Func<TKey, TValue> defaultValueFn)
        {
            TValue value;
            return !dict.TryGetValue(key, out value) ? defaultValueFn(key) : value;
        }
    }
}
