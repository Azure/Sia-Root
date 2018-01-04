using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Sia.Shared.Protocol.Pagination
{
    public interface IPaginationByCursorSelectors<TSource, TDestination, TCursor>
    {
        Func<TDestination, TCursor> DtoValueSelector { get; }
        Expression<Func<TSource, TCursor>> DataValueSelector { get; }
        Func<TSource, TCursor> CompiledCursorSelector { get; }
    }

    public abstract class PaginationByCursorSelectors<TSource, TDestination, TCursor>
        : IPaginationByCursorSelectors<TSource, TDestination, TCursor>
    {
        public Func<TSource, TCursor> CompiledCursorSelector
        {
            get
            {
                if (_compiledCursorSelector is null)
                {
                    _compiledCursorSelector = DataValueSelector.Compile();
                }
                return _compiledCursorSelector;
            }
        }

        private Func<TSource, TCursor> _compiledCursorSelector;
        public abstract Expression<Func<TSource, TCursor>> DataValueSelector { get; }
        public abstract Func<TDestination, TCursor> DtoValueSelector { get; }
    }
}
