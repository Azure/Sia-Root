using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Sia.Shared.Data;
using Newtonsoft.Json;
using System.Linq;

namespace Sia.Shared.Protocol
{
    public class FilteredLinksHeader : LinksHeader
    {
        public FilteredLinksHeader(IFilterMetadataProvider filterMetadata, PaginationMetadata metadata, IUrlHelper urlHelper, string routeName, long incidentId)
            : base(metadata, urlHelper, routeName, incidentId)
        {
            _filterMetadata = filterMetadata;
        }

        protected IFilterMetadataProvider _filterMetadata;

        protected override IEnumerable<KeyValuePair<string, string>> _headerValues() 
            => base._headerValues().Concat(_filterMetadata.FilterValues());

        protected override IEnumerable<KeyValuePair<string, string>> _nextPageLinkValues
            => base._nextPageLinkValues.Concat(_filterMetadata.FilterValues());

        protected override IEnumerable<KeyValuePair<string, string>> _previousPageLinkValues
           => base._previousPageLinkValues.Concat(_filterMetadata.FilterValues());

    }
}
