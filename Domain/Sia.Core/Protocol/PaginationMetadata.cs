using Sia.Core.Protocol;
using System.Collections.Generic;
using System.Globalization;

namespace Sia.Core.Protocol
{
    public class PaginationMetadata
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 50;
        public long TotalRecords { get; set; }
        public long TotalPages => (TotalRecords / PageSize) + (TotalRecords % PageSize > 0 ? 1 : 0);


        public IDictionary<string, string> PreviousPageLinkInfo => new Dictionary<string, string>
        {
            { nameof(PageNumber), (PageNumber - 1).ToString(CultureInfo.InvariantCulture) },
            { nameof(PageSize), PageSize.ToString(CultureInfo.InvariantCulture) }
        };


        public IDictionary<string, string> NextPageLinkInfo => new Dictionary<string, string>
        {
            { nameof(PageNumber), (PageNumber + 1).ToString(CultureInfo.InvariantCulture) },
            { nameof(PageSize), PageSize.ToString(CultureInfo.InvariantCulture) }
        };

        public bool PreviousPageExists => PageNumber > 1;
        public bool NextPageExists => PageNumber < TotalPages;
    }
}

namespace System.Linq
{
    public static class PaginationExtensions
    {
        public static IQueryable<T> WithPagination<T>(this IQueryable<T> source, PaginationMetadata pagination)
        {
            pagination.TotalRecords = source.LongCount();
            return source.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize);
        }
    }
}
