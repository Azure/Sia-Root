using System;
using System.Collections.Generic;
using System.Text;

namespace System.Collections.Concurrent
{
    public static class ConcurrentCollectionExtensions
    {
        /// <summary>
        /// Minor wrapper around AddOrUpdate inteded for use when the passed in value should
        /// always override the existing value in the dictionary.
        /// 
        /// WARNING: This has a high risk of causing race conditions when multiple threads are upserting simultaneously
        /// </summary>
        /// <returns>The final value in the concurrent dictionary (which may not be the passed in expected final value)</returns>
        public static TValue Upsert<TKey, TValue>(
            this ConcurrentDictionary<TKey, TValue> dictionary,
            TKey keyToUpdate,
            TValue intendedFinalValue
        ) => dictionary.AddOrUpdate(keyToUpdate, intendedFinalValue, (key, value) => intendedFinalValue);
    }
}

namespace System.Collections.Generic
{
    public static class CollectionExtensions
    {
        public static IDictionary<string, TValue> ToDictionary<TValue>(this IEnumerable<KeyValuePair<string, TValue>> entries)
        {
            var toReturn = new Dictionary<string, TValue>(StringComparer.InvariantCultureIgnoreCase);
            foreach (var entry in entries)
            {
                if(!toReturn.ContainsKey(entry.Key))
                {
                    toReturn.Add(entry.Key, entry.Value);
                }
            }
            return toReturn;
        }
    }
}