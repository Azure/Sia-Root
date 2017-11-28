using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Sia.Shared.Data;
using Newtonsoft.Json;

namespace Sia.Shared.Protocol
{
    public class FilteredLinksHeader<T> : LinksHeader
    {
        public FilteredLinksHeader(Filters<T> filterMetadata, PaginationMetadata metadata, IUrlHelper urlHelper, string routeName)
            : base(metadata, urlHelper, routeName)
        {
            _filterMetadata = filterMetadata;
        }

        private Filters<T> _filterMetadata;

        public override StringValues HeaderValues
            => StringValues.Concat(base.HeaderValues, _filterMetadata.FilterValues());

        protected override StringValues NextPageLinkInfo
            => StringValues.Concat(base.NextPageLinkInfo, _filterMetadata.FilterValues());

        protected override StringValues PreviousPageLinkInfo
            => StringValues.Concat(base.PreviousPageLinkInfo, _filterMetadata.FilterValues());
    }
}
