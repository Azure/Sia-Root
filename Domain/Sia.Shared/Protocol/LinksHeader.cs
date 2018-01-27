using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;
using Sia.Shared.Data;

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
        public string HeaderJson => JsonConvert.SerializeObject(GetHeaderValues(), _serializationSettings());
        protected JsonSerializerSettings _serializationSettings()
            => new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            };
        public LinksForSerialization GetHeaderValues()
        {
            var toReturn = new LinksForSerialization();
            toReturn.Metadata = _metadata is null
                ? null
                : new Metadata()
                {
                    Pagination = new PaginationMetadataRecord()
                    {
                        PageNumber = _metadata.PageNumber.ToString(),
                        PageSize = _metadata.PageSize.ToString(),
                        TotalRecords = _metadata.TotalRecords.ToString(),
                        TotalPages = _metadata.TotalPages.ToString()
                    }
                };
            toReturn.Links = new LinksCollection()
            {
                Operations = _operationLinks,
                Pagination = (_metadata == null || (!_metadata.PreviousPageExists && !_metadata.NextPageExists)) && _filterMetadata == null
                    ? null
                    : new PaginationLinks()
                    {
                        Previous = _previousPageLink,
                        Next = _nextPageLink
                    },
                Related = _relationLinks
            };
            return toReturn;
        }


        protected string _nextPageLink => _metadata.NextPageExists
            ? _urlHelper.Link(_routeName, new { }) + FormatUrl(_nextPageLinkValues())
            : null;
        protected string _previousPageLink => _metadata.PreviousPageExists
            ? _urlHelper.Link(_routeName, new { }) + FormatUrl(_previousPageLinkValues())
            : null;

        protected virtual IEnumerable<KeyValuePair<string, string>> _nextPageLinkValues()
        {
            var nextPageLinksValue = _metadata.NextPageLinkInfo;
            if (_filterMetadata != null)
            {
                nextPageLinksValue.Concat(_filterMetadata.FilterValues());
            }
            return nextPageLinksValue;
        }

        protected virtual IEnumerable<KeyValuePair<string, string>> _previousPageLinkValues()
        {
            var previousPageLinksValue = _metadata.PreviousPageLinkInfo;
            if (_filterMetadata != null)
            {
                previousPageLinksValue.Concat(_filterMetadata.FilterValues());
            }

            return previousPageLinksValue;
        }

        protected string UrlTokenFormat(KeyValuePair<string, string> token)
            => $"{token.Key}={token.Value}";

        protected string FormatUrl(IEnumerable<KeyValuePair<string, string>> tokens)
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