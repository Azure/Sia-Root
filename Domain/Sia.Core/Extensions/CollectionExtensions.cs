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
        /// </summary>
        /// <returns>The final value in the concurrent dictionary (which may not be the passed in expected final value)</returns>
        public static TValue Upsert<TKey, TValue>(
            this ConcurrentDictionary<TKey, TValue> dictionary,
            TKey keyToUpdate,
            TValue intendedFinalValue
        ) => dictionary.AddOrUpdate(keyToUpdate, intendedFinalValue, (key, value) => intendedFinalValue);
    }
}
