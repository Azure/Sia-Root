using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sia.Core.Data;
using Sia.Core.Validation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Sia.Core.Protocol
{
    public interface ILinksHeader
    {
        LinksForSerialization GetHeaderValues();
    }
    
    public class CrudLinksHeader : ILinksHeader
    {
        private OperationLinks OperationLinks { get; }
        private RelationLinks RelationLinks { get; }

        public CrudLinksHeader(OperationLinks operationLinks, RelationLinks relationLinks)
        {
            OperationLinks = ThrowIf.Null(operationLinks, nameof(operationLinks));
            RelationLinks = relationLinks;
        }

        public LinksForSerialization GetHeaderValues() => 
            new LinksForSerialization()
            {
                Links = new LinksCollection()
                {
                    Operations = OperationLinks,
                    Related = RelationLinks
                }
            };
    }

    public class PaginatedLinksHeader : ILinksHeader
    {
        private IFilterMetadataProvider _filterMetadata { get; }
        private PaginationMetadata _pagination { get; }
        private string _baseRoute { get; }
        private OperationLinks _operationLinks { get; }
        private RelationLinks _relationLinks { get; }

        public PaginatedLinksHeader(
            string baseRoute,
            OperationLinks operationLinks,
            RelationLinks relationLinks,
            PaginationMetadata pagination,
            IFilterMetadataProvider filterMetadata = null
        )
        {
            _operationLinks = ThrowIf.Null(operationLinks, nameof(operationLinks));
            _relationLinks = relationLinks;
            _baseRoute = ThrowIf.NullOrWhiteSpace(baseRoute, nameof(baseRoute));
            _pagination = ThrowIf.Null(pagination, nameof(pagination));
            _filterMetadata = filterMetadata ?? new NoFilterMetadata();
        }

        public const string HeaderName = "links";
            
        public LinksForSerialization GetHeaderValues()
        {
            var toReturn = new LinksForSerialization
            {
                Metadata = new LinksMetadata()
                {
                    Pagination = _pagination.ToSerializableValues()
                },
                Links = new LinksCollection()
                {
                    Operations = _operationLinks,
                    Pagination = new PaginationLinks()
                    {
                        Previous = MakeDirectionalLink(_pagination.Previous),
                        Next = MakeDirectionalLink(_pagination.Next)
                    },
                    Related = _relationLinks
                }
            };
            return toReturn;
        }

        protected string MakeDirectionalLink(DirectionalPaginationMetadata page)
            => page.Exists
                ? _baseRoute + FormatUrl(page.LinkInfo.Concat(_filterMetadata.FilterValues()))
                : null;

        protected static string UrlTokenFormat(KeyValuePair<string, string> token)
            => $"{token.Key}={token.Value}";

        protected static string FormatUrl(IEnumerable<KeyValuePair<string, string>> tokens)
            => "/?" + string.Join("&", tokens.Select(UrlTokenFormat));
    }
}

namespace Microsoft.AspNetCore.Mvc
{
    using Microsoft.AspNetCore.Http;
    using Sia.Core.Protocol;
    using Newtonsoft.Json;

    public static class PaginationExtensions
    {
        public static JsonSerializerSettings SerializationSettings { get; }
            = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            };
        public static void AddLinksHeader(this IHeaderDictionary headers, ILinksHeader header)
        {
            var headerJson = JsonConvert.SerializeObject(header.GetHeaderValues(), SerializationSettings);

            headers.Add("Access-Control-Expose-Headers", PaginatedLinksHeader.HeaderName);
            headers.Add(PaginatedLinksHeader.HeaderName, headerJson);
        }
    }
}