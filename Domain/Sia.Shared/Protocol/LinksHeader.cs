using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sia.Shared.Protocol
{
    public class LinksHeader
    {
        private PaginationMetadata _metadata;
        private IUrlHelper _urlHelper;
        private string _routeName;
        private readonly OperationLinks _operationLinks;
        private readonly RelationLinks _relationLinks;

        public LinksHeader(
            PaginationMetadata metadata,
            IUrlHelper urlHelper,
            string routeName, 
            OperationLinks operationLinks,
            RelationLinks relationLinks)
        {
            _metadata = metadata;
            _urlHelper = urlHelper;
            _routeName = routeName;
            _operationLinks = operationLinks;
            _relationLinks = relationLinks;
        }

        public const string HeaderName = "links";
        public string HeaderJson => JsonConvert.SerializeObject(getHeaderValues(), _serializationSettings());
        protected JsonSerializerSettings _serializationSettings()
            => new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            };
        protected LinksForSerialization getHeaderValues()
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
                Pagination = _metadata is null
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
            ? _urlHelper.Action(_routeName) + FormatUrl(_nextPageLinkValues)
            : null;
        protected string _previousPageLink => _metadata.PreviousPageExists
            ? _urlHelper.Action(_routeName) + FormatUrl(_previousPageLinkValues)
            : null;

        protected virtual IEnumerable<KeyValuePair<string, string>> _nextPageLinkValues
            => _metadata.NextPageLinkInfo;
        protected virtual IEnumerable<KeyValuePair<string, string>> _previousPageLinkValues
            => _metadata.PreviousPageLinkInfo;

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