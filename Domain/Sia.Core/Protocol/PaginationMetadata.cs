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

        public DirectionalPaginationMetadata Previous => new DirectionalPaginationMetadata()
        {
            Exists = PageNumber > 1,
            LinkInfo = new Dictionary<string, string>
            {
                { nameof(PageNumber), (PageNumber - 1).ToString(CultureInfo.InvariantCulture) },
                { nameof(PageSize), PageSize.ToString(CultureInfo.InvariantCulture) }
            }
        };

        public DirectionalPaginationMetadata Next => new DirectionalPaginationMetadata()
        {
            Exists = PageNumber < TotalPages,
            LinkInfo = new Dictionary<string, string>
            {
                { nameof(PageNumber), (PageNumber + 1).ToString(CultureInfo.InvariantCulture) },
                { nameof(PageSize), PageSize.ToString(CultureInfo.InvariantCulture) }
            }
        };

    }

    public class DirectionalPaginationMetadata
    {
        public bool Exists { get; set; }
        public IDictionary<string, string> LinkInfo { get; set; }
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

        public static PaginationMetadataValues ToSerializableValues(this PaginationMetadata pagination)
            => new PaginationMetadataValues()
            {
                PageNumber = pagination.PageNumber.ToPathTokenString(),
                PageSize = pagination.PageSize.ToPathTokenString(),
                TotalRecords = pagination.TotalRecords.ToPathTokenString(),
                TotalPages = pagination.TotalPages.ToPathTokenString()
            };
    }
}
