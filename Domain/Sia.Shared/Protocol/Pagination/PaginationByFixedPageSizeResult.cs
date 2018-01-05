using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Shared.Protocol.Pagination
{
    public class PaginationByFixedPageSizeResult<TSource, TDestination>
        : IPaginationResultMetadata<TDestination>
    {
        public PaginationByFixedPageSizeResult(
            PaginationByFixedPageSizeRequest<TSource, TDestination> request,
            List<TDestination> queryResult,
            int totalRecords)
        {
            _request = request;
            TotalRecords = totalRecords;
            QueryResult = queryResult;
        }
        protected PaginationByFixedPageSizeRequest<TSource, TDestination> _request { get; }
        public List<TDestination> QueryResult { get; }
        public int MaxPageSize => _request.MaxPageSize;
        public long TotalRecords { get; }
        public long TotalPages => (TotalRecords / MaxPageSize) + (TotalRecords % MaxPageSize > 0 ? 1 : 0);
        public bool PreviousPageExists => _request.PageNumber > 1;
        public bool NextPageExists => _request.PageNumber < TotalPages;

        public IEnumerable<KeyValuePair<string, string>> PreviousPageLinkInfo
        {
            get
            {
                foreach (var item in SharedHeaderValues())
                {
                    yield return item;
                }
                yield return new KeyValuePair<string, string>(nameof(_request.PageNumber), (_request.PageNumber - 1).ToString());
            }
        }

        public IEnumerable<KeyValuePair<string, string>> NextPageLinkInfo
        {
            get
            {
                foreach (var item in SharedHeaderValues())
                {
                    yield return item;
                }
                yield return new KeyValuePair<string, string>(nameof(_request.PageNumber), (_request.PageNumber + 1).ToString());
            }
        }

        public IEnumerable<KeyValuePair<string, string>> PaginationHeaderValues()
        {
            foreach (var item in SharedHeaderValues())
            {
                yield return item;
            }
            yield return new KeyValuePair<string, string>(nameof(_request.PageNumber), _request.PageNumber.ToString());
        }

        protected IEnumerable<KeyValuePair<string, string>> SharedHeaderValues()
        {
            yield return new KeyValuePair<string, string>(nameof(MaxPageSize), MaxPageSize.ToString());
            yield return new KeyValuePair<string, string>(nameof(TotalRecords), TotalRecords.ToString());
            yield return new KeyValuePair<string, string>(nameof(TotalPages), TotalPages.ToString());
        }

    }
}
