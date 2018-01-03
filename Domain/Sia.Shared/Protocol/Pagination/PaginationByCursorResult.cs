using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sia.Shared.Protocol.Pagination
{
    public class PaginationByCursorResult<TSource, TDestination, TCursor>
        : IPaginationResultMetadata<TDestination>
        where TCursor : IComparable<TCursor>
    {
        public PaginationByCursorResult(
            PaginationByCursorRequest<TSource, TDestination, TCursor> request,
            int totalRecords,
            List<TDestination> queryResult)
        {
            _request = request;
            TotalRecords = totalRecords;
            QueryResult = queryResult;
        }

        private readonly PaginationByCursorRequest<TSource, TDestination, TCursor> _request;
        public List<TDestination> QueryResult { get; }
        public int MaxPageSize => _request.MaxPageSize;
        public long TotalRecords { get; }
        public long TotalPages => (TotalRecords / MaxPageSize) + (TotalRecords % MaxPageSize > 0 ? 1 : 0);
        public TCursor FinalValue 
            => _request.CursorDirectionBool
                ? QueryResult.Max(_request.DtoValueSelector) //asc
                : QueryResult.Min(_request.DtoValueSelector);//desc
        public bool ReadingInOrder => _request.SortOrderBool == _request.CursorDirectionBool;

        public bool NextPageExists
            => !(ReadingInOrder
                && _request.CursorValue.Equals(FinalValue));
        public bool PreviousPageExists
            => _request.CursorValue.Equals(default(TCursor)) ? false //Reading first page
            : ReadingInOrder ? true //Reading page after the first
            : _request.CursorValue.Equals(FinalValue); //?

        public IEnumerable<KeyValuePair<string, string>> NextPageLinkInfo
        {
            get
            {
                foreach (var item in SharedHeaderValues())
                {
                    yield return item;
                }
                var cursorDirection = _request.TranslateFromDirectionalBool(
                    ReadingInOrder
                    ? _request.CursorDirectionBool
                    : !_request.CursorDirectionBool);
                var cursorValue = ReadingInOrder
                    ? FinalValue
                    : _request.CursorValue;
                yield return new KeyValuePair<string, string>(nameof(_request.CursorDirection), cursorDirection);
                yield return new KeyValuePair<string, string>(nameof(_request.CursorValue), cursorValue.ToString());
            }
        }

        public IEnumerable<KeyValuePair<string, string>> PreviousPageLinkInfo
        {
            get
            {
                foreach (var item in SharedHeaderValues())
                {
                    yield return item;
                }
                var cursorDirection = _request.TranslateFromDirectionalBool(!_request.CursorDirectionBool);
                var cursorValue = ReadingInOrder
                    ? _request.CursorValue
                    : FinalValue;
                yield return new KeyValuePair<string, string>(nameof(_request.CursorDirection), cursorDirection);
                yield return new KeyValuePair<string, string>(nameof(_request.CursorValue), cursorValue.ToString());
            }
        }

        public IEnumerable<KeyValuePair<string, string>> PaginationHeaderValues()
        {
            foreach (var item in SharedHeaderValues())
            {
                yield return item;
            }
        }

        protected IEnumerable<KeyValuePair<string, string>> SharedHeaderValues()
        {
            var cursorDirection = ReadingInOrder
                    ? _request.TranslateFromDirectionalBool(!_request.CursorDirectionBool)
                    : _request.CursorDirection;
            yield return new KeyValuePair<string, string>(nameof(MaxPageSize), MaxPageSize.ToString());
            yield return new KeyValuePair<string, string>(nameof(TotalRecords), TotalRecords.ToString());
            yield return new KeyValuePair<string, string>(nameof(TotalPages), TotalPages.ToString());
            yield return new KeyValuePair<string, string>(nameof(_request.SortOrder), _request.SortOrder);
        }
    }
}
