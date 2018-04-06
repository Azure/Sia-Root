using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sia.Shared.Data;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Sia.Shared.Protocol
{
    public class LinksHeader
    {
        private IFilterMetadataProvider _filterMetadata;
        private PaginationMetadata _metadata;
        private IUrlHelper _urlHelper;
        private string _routeName;
        private readonly OperationLinks _operationLinks;
        private readonly RelationLinks _relationLinks;

        public LinksHeader(
            IFilterMetadataProvider filterMetadata,
            PaginationMetadata metadata,
            IUrlHelper urlHelper,
            string routeName, 
            OperationLinks operationLinks,
            RelationLinks relationLinks)
        {
            _filterMetadata = filterMetadata;
            _metadata = metadata;
            _urlHelper = urlHelper;
            _routeName = routeName;
            _operationLinks = operationLinks;
            _relationLinks = relationLinks;
        }

        public const string HeaderName = "links";
        public string HeaderJson => JsonConvert.SerializeObject(GetHeaderValues(), serializationSettings());
        protected JsonSerializerSettings serializationSettings()
            => new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            };
        public LinksForSerialization GetHeaderValues()
        {
            var toReturn = new LinksForSerialization();
            toReturn.Metadata = _metadata is null
                ? null
                : new LinksMetadata()
                {
                    Pagination = new PaginationMetadataRecord()
                    {
                        PageNumber = _metadata.PageNumber.ToString(CultureInfo.InvariantCulture),
                        PageSize = _metadata.PageSize.ToString(CultureInfo.InvariantCulture),
                        TotalRecords = _metadata.TotalRecords.ToString(CultureInfo.InvariantCulture),
                        TotalPages = _metadata.TotalPages.ToString(CultureInfo.InvariantCulture)
                    }
                };
            toReturn.Links = new LinksCollection()
            {
                Operations = _operationLinks,
                Pagination = (_metadata == null || (!_metadata.PreviousPageExists && !_metadata.NextPageExists)) && _filterMetadata == null
                    ? null
                    : new PaginationLinks()
                    {
                        Previous = previousPageLink,
                        Next = nextPageLink
                    },
                Related = _relationLinks
            };
            return toReturn;
        }


        protected string nextPageLink => _metadata.NextPageExists
            ? _urlHelper.Link(_routeName, new { }) + FormatUrl(nextPageLinkValues())
            : null;
        protected string previousPageLink => _metadata.PreviousPageExists
            ? _urlHelper.Link(_routeName, new { }) + FormatUrl(previousPageLinkValues())
            : null;

        protected virtual IEnumerable<KeyValuePair<string, string>> nextPageLinkValues()
        {
            var nextPageLinksValue = _metadata.NextPageLinkInfo;
            if (_filterMetadata != null)
            {
                nextPageLinksValue.Concat(_filterMetadata.FilterValues());
            }
            return nextPageLinksValue;
        }

        protected virtual IEnumerable<KeyValuePair<string, string>> previousPageLinkValues()
        {
            var previousPageLinksValue = _metadata.PreviousPageLinkInfo;
            if (_filterMetadata != null)
            {
                previousPageLinksValue.Concat(_filterMetadata.FilterValues());
            }

            return previousPageLinksValue;
        }

        protected static string UrlTokenFormat(KeyValuePair<string, string> token)
            => $"{token.Key}={token.Value}";

        protected static string FormatUrl(IEnumerable<KeyValuePair<string, string>> tokens)
            => "/?" + string.Join("&", tokens.Select(UrlTokenFormat));
    }
}

namespace Microsoft.AspNetCore.Mvc
{
    using Microsoft.AspNetCore.Http;
    using Sia.Shared.Protocol;

    public static class PaginationExtensions
    {
        public static void AddLinksHeader(this IHeaderDictionary headers, LinksHeader header)
        {
            headers.Add("Access-Control-Expose-Headers", LinksHeader.HeaderName);
            headers.Add(LinksHeader.HeaderName, header.HeaderJson);
        }
    }
}