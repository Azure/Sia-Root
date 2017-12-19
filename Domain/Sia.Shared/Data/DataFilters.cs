using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sia.Shared.Data
{
    public abstract class DataFilters<T> : Filters<T>
        where T : IJsonDataString
    {
        public string DataKey { get; set; }
        public string DataValue { get; set; }
        public const string KeyValueComparison = "\"{0}\":\"{1}\"";
        public const string KeyComparison = "\"{0}\":";
        public override IQueryable<T> Filter(IQueryable<T> source)
        {
            var working = source;
            if (!String.IsNullOrEmpty(DataKey))
            {
                var workingCompare = String.IsNullOrEmpty(DataValue)
                    ? String.Format(KeyComparison, DataKey)
                    : String.Format(KeyValueComparison, new string[] { DataKey, DataValue });
                working = working.Where(obj => obj.Data.Contains(workingCompare));
            }
            return working;
        }
        public abstract IEnumerable<KeyValuePair<string, string>> NonDataFilterValues();
        public override IEnumerable<KeyValuePair<string, string>> FilterValues()
            => _dataFilterValues().Concat(NonDataFilterValues());

        private IEnumerable<KeyValuePair<string, string>> _dataFilterValues()
        {
            if (!string.IsNullOrWhiteSpace(DataKey)) yield return new KeyValuePair<string, string>(nameof(DataKey), DataKey);
            if (!string.IsNullOrWhiteSpace(DataValue)) yield return new KeyValuePair<string, string>(nameof(DataValue), DataValue );
        }
    }
}
