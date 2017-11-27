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
    }
}
