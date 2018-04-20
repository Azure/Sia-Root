namespace Sia.Core.Protocol
{
    using Sia.Core.Data;
    using Sia.Core.Validation;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Encodings.Web;

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
            => $"{UrlEncoder.Default.Encode(token.Key)}={UrlEncoder.Default.Encode(token.Value)}";

        protected static string FormatUrl(IEnumerable<KeyValuePair<string, string>> tokens)
            => "/?" + string.Join("&", tokens.Select(UrlTokenFormat));
    }
}
