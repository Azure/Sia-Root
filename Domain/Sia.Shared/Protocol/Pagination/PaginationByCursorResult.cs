using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sia.Shared.Protocol.Pagination
{
    public class PaginationByCursorResult<TSource, TDestination, TCursor>
        : IPaginationResultMetadata<TDestination>
        where TCursor : IComparable<TCursor>, IPaginationCursor<TCursor>
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
        public TCursor FirstResult => _request.DtoValueSelector(QueryResult[0]);

        public TCursor LastResult  => _request.DtoValueSelector(QueryResult[QueryResult.Count - 1]);
        public bool ReadingInOrder => _request.SortOrderBool == _request.SeekDirectionBool;

        public bool NextPageExists
            => true;
            //!(ReadingInOrder && _request.CursorValue.Equals(MaxResult));
        public bool PreviousPageExists
            => true;
            /*_request.CursorValue.Equals(default(TCursor)) ? false //Reading first page
            : ReadingInOrder ? true //Reading page after the first
            : _request.CursorValue.Equals(MaxResult); //?*/

        public IEnumerable<KeyValuePair<string, string>> NextPageLinkInfo
        {
            get
            {
                foreach (var item in SharedHeaderValues())
                {
                    yield return item;
                }
                foreach (var item in LastResult.SerializationTokens())
                {
                    yield return item;
                }
                var cursorDirection = _request.TranslateFromDirectionalBool(_request.SeekDirectionBool);

                yield return new KeyValuePair<string, string>(nameof(_request.SeekDirection), cursorDirection);
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
                foreach (var item in FirstResult.SerializationTokens())
                {
                    yield return item;
                }
                var cursorDirection = _request.TranslateFromDirectionalBool(!_request.SeekDirectionBool);
                    
                yield return new KeyValuePair<string, string>(nameof(_request.SeekDirection), cursorDirection);
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
                    ? _request.TranslateFromDirectionalBool(!_request.SeekDirectionBool)
                    : _request.SeekDirection;
            yield return new KeyValuePair<string, string>(nameof(MaxPageSize), MaxPageSize.ToString());
            yield return new KeyValuePair<string, string>(nameof(TotalRecords), TotalRecords.ToString());
            yield return new KeyValuePair<string, string>(nameof(TotalPages), TotalPages.ToString());
            yield return new KeyValuePair<string, string>(nameof(_request.SortOrder), _request.SortOrder);
        }
    }
}
