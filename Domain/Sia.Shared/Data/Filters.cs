using Microsoft.Extensions.Primitives;
using Sia.Shared.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sia.Shared.Data
{
    public abstract class Filters<T>
    {
        public abstract IQueryable<T> Filter(IQueryable<T> source);
        public abstract StringValues FilterValues();
    }
}

namespace System.Linq
{
    public static class FilterExtensions
    {
        public static IQueryable<T> WithFilter<T>(this IQueryable<T> source, Filters<T> filter)
            => filter.Filter(source);
    }
}