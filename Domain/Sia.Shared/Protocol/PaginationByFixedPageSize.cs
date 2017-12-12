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
        protected override StringValues NextPageLinkValues
            => JsonConvert.SerializeObject(new
            {
                PageNumber = PageNumber + 1
            });
        protected override StringValues PreviousPageLinkValues
            => JsonConvert.SerializeObject(new
            {
                PageNumber = PageNumber - 1
            });
        protected override IQueryable<T> ImplementPagination(IQueryable<T> source)
            => source
                .Skip((PageNumber - 1) * MaxPageSize)
                .Take(MaxPageSize);
        public override StringValues HeaderValues(IUrlHelper urlHelper, string routeName)
            => StringValues.Concat(
                    base.HeaderValues(urlHelper, routeName),
                    JsonConvert.SerializeObject(new
                    {
                        PageNumber = PageNumber
                    })
                );

    }
}
