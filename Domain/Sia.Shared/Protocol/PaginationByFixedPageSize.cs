using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Sia.Shared.Protocol
{
    public class PaginationByFixedPageSize<T>
        : PaginationMetadata<T>
    {
        public int PageNumber { get; set; } = 1;

        public override bool PreviousPageExists => PageNumber > 1;
        public override bool NextPageExists => PageNumber < TotalPages;

        public override IEnumerable<KeyValuePair<string, string>> PreviousPageLinkInfo
        {
            get
            {
                yield return new KeyValuePair<string, string>(nameof(PageNumber), (PageNumber - 1).ToString() );
            }
        }

        public override IEnumerable<KeyValuePair<string, string>> NextPageLinkInfo
        {
            get
            {
                yield return new KeyValuePair<string, string>(nameof(PageNumber), (PageNumber + 1).ToString() );
            }
        }
        public override IQueryable<T> Paginate(IQueryable<T> source)
            => source
                .Skip((PageNumber - 1) * MaxPageSize)
                .Take(MaxPageSize);

        public override IEnumerable<KeyValuePair<string, string>> PaginationHeaderValues()
        {
            foreach (var value in base.PaginationHeaderValues())
            {
                yield return value;
            }
            yield return new KeyValuePair<string, string>(nameof(PageNumber), PageNumber.ToString());
        }

    }
}
