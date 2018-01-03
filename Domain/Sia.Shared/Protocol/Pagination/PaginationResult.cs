using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sia.Shared.Protocol.Pagination
{
    public interface IPaginationResultMetadata<TResult>
        : IPaginationLinkValues
    {
        List<TResult> QueryResult { get; }

        long TotalRecords { get; }
        long TotalPages { get; }
        int MaxPageSize { get; }
    }

    public interface IPaginationLinkValues
    {
        IEnumerable<KeyValuePair<string, string>> PaginationHeaderValues();
        IEnumerable<KeyValuePair<string, string>> PreviousPageLinkInfo { get; }
        IEnumerable<KeyValuePair<string, string>> NextPageLinkInfo { get; }
        bool NextPageExists { get; }
        bool PreviousPageExists { get; }
    }
}
