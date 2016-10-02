using System.Collections.Generic;

namespace SharpEnd
{
    internal static class DictionaryExtensions
    {
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue def)
        {
            TValue result;

            return dictionary.TryGetValue(key, out result) ? result : def;
        }
    }
}
