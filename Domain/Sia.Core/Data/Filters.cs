using Microsoft.Extensions.Primitives;
using Sia.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sia.Core.Data
{
    public interface IFilterByMatch<T>
    {
        bool IsMatchFor(T toCompare);
    }

    public interface IFilterMetadataProvider
    {
        IEnumerable<KeyValuePair<string, string>> FilterValues();
    }

    public class NoFilterMetadata : IFilterMetadataProvider
    {
        public IEnumerable<KeyValuePair<string, string>> FilterValues()
            => Enumerable.Empty<KeyValuePair<string, string>>();
    }

    public interface IFilters<T>
        : IFilterByMatch<T>,
        IFilterMetadataProvider
    {}
}

namespace System.Linq
{
    public static class FilterExtensions
    {
        public static IQueryable<T> WithFilter<T>(this IQueryable<T> source, IFilterByMatch<T> filter)
            => filter == null
                ? throw new ArgumentNullException(nameof(filter))
                : source.Where(t => filter.IsMatchFor(t));
    }
}