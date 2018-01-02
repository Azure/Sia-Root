using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Primitives;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace Sia.Shared.Protocol
{
    public abstract class PaginationByCursor<TEntity, TDTO, TCursor>
        : PaginationMetadata<TEntity>,
        ITrackingPaginator<TEntity, TDTO>
        where TCursor : IComparable<TCursor>
    {
        public List<TDTO> QueryResult { get; set; }
        public TCursor CursorValue { get; set; }
        public TCursor FinalValue
        {
            get
            {
                if(_finalValue.Equals(default(TCursor)))
                {
                    if(QueryResult is null) throw PaginationUsageException.NeedsQueryResult;

                    _finalValue = _cursorDirection
                        ? QueryResult.Max(DtoValueSelector) //asc
                        : QueryResult.Min(DtoValueSelector);//desc
                }
                return _finalValue;
            }
        }
        
        /// <summary>
        /// If "asc", result values will be greater than or equal to CursorValue,
        /// if "desc", result values will be less than or equal to CursorValue
        /// </summary>
        public string CursorDirection
        {
            get => TranslateFromDirectionalBool(_cursorDirection); 
            set => _cursorDirection = TranslateToDirectionalBool(value);
        }
        /// <summary>
        /// If "asc", next page values will be greater than result values,
        /// if "desc", next page values will be less than result values.
        /// Vice versa for previous page values.
        /// Also determines sort order on the result set.
        /// </summary>
        public string SortOrder {
            get => TranslateFromDirectionalBool(_sortOrder);
            set => _sortOrder = TranslateToDirectionalBool(value);
        }
        private bool _cursorDirection = false;
        private bool _sortOrder = false;
        private TCursor _finalValue;

        protected bool _readingInOrder => _sortOrder == _cursorDirection;
        protected bool _readingInReverse => _sortOrder != _cursorDirection;

        public override bool NextPageExists
            => !(_cursorDirection == _sortOrder 
                && CursorValue.Equals(FinalValue));
        public override bool PreviousPageExists
            => CursorValue.Equals(default(TCursor)) ? false //Reading first page
            : _cursorDirection == _sortOrder ? true //Reading page after the first
            : CursorValue.Equals(FinalValue); //?

        public override IEnumerable<KeyValuePair<string, string>> NextPageLinkInfo
        {
            get
            {
                var cursorDirection = TranslateFromDirectionalBool(_readingInOrder
                    ? _cursorDirection
                    : !_cursorDirection);
                var cursorValue = _readingInOrder
                    ? FinalValue
                    : CursorValue;
                yield return new KeyValuePair<string, string>(nameof(CursorDirection), cursorDirection);
                yield return new KeyValuePair<string, string>(nameof(CursorValue), cursorValue.ToString());
                yield return new KeyValuePair<string, string>(nameof(SortOrder), SortOrder);
            }
        }

        public override IEnumerable<KeyValuePair<string, string>> PreviousPageLinkInfo
        {
            get
            {
                var cursorDirection = _readingInOrder
                    ? TranslateFromDirectionalBool(!_cursorDirection)
                    : CursorDirection;
                var cursorValue = _readingInOrder
                    ? CursorValue
                    : FinalValue;
                yield return new KeyValuePair<string, string>(nameof(CursorDirection), cursorDirection);
                yield return new KeyValuePair<string, string>(nameof(CursorValue), cursorValue.ToString());
                yield return new KeyValuePair<string, string>(nameof(SortOrder), SortOrder);
            }
        }

        public override IQueryable<TEntity> Paginate(IQueryable<TEntity> source)
        {
            var filteredResults = source
                .Where(ValidRecord);
            var orderedResults = _sortOrder
                ? filteredResults.OrderBy(DataValueSelector)
                : filteredResults.OrderByDescending(DataValueSelector);
            return orderedResults.Take(MaxPageSize);
        }

        protected Expression<Func<TEntity, bool>> ValidRecord
            => (record)
            => _cursorDirection
            ? CompiledCursorSelector(record).CompareTo(CursorValue) <= 0
                //Return values are equal to or greater than CursorValue
            : CompiledCursorSelector(record).CompareTo(CursorValue) >= 0;
                //Return values are equal to or less than CursorValue

        protected abstract Expression<Func<TEntity, TCursor>> DataValueSelector { get; }
        protected abstract Func<TDTO, TCursor> DtoValueSelector { get; }
        protected Func<TEntity, TCursor> CompiledCursorSelector {
            get
            {
                if(_compiledCursorSelector is null)
                {
                    _compiledCursorSelector = DataValueSelector.Compile();
                }
                return _compiledCursorSelector;
            }
        }

        private Func<TEntity, TCursor> _compiledCursorSelector;

        private string TranslateFromDirectionalBool(bool value)
            => value
                ? "asc"
                : "desc";
        private bool TranslateToDirectionalBool(string value)
            => value.Equals("asc")
                ? true
                : false;
    }

    public class PaginationUsageException : Exception
    {
        public PaginationUsageException(string message)
            : base(message) { }

        public static PaginationUsageException NeedsQueryResult
            => new PaginationUsageException(
                "ITrackingPaginator instances need query result "
                + "provided by PaginationExtensions.GetPageAsync. "
                + "FinalValue and properties based on it cannot "
                + "be accessed until the query is complete");
    }
}
