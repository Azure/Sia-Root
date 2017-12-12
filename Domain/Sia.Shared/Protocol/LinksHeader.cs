using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Sia.Shared.Validation;

namespace Sia.Shared.Protocol
{
    public class LinksHeader
    {
        private IPaginationLinks _metadata;
        private IUrlHelper _urlHelper;
        private string _routeName;

        public LinksHeader(IPaginationLinks metadata, IUrlHelper urlHelper, string routeName)
        {
            _metadata = ThrowIf.Null(metadata, nameof(metadata));
            _urlHelper = ThrowIf.Null(urlHelper, nameof(urlHelper));
            _routeName = ThrowIf.NullOrWhiteSpace(routeName, nameof(routeName));
        }

        public const string HeaderName = "links";
        public virtual StringValues HeaderValues =>
            _metadata.HeaderValues(_urlHelper, _routeName);
    }
}

namespace Microsoft.AspNetCore.Mvc
{
    using Microsoft.AspNetCore.Http;
    using Sia.Shared.Protocol;

    public static class PaginationExtensions
    {
        public static void AddPagination(this IHeaderDictionary headers, LinksHeader header)
        {
            headers.Add("Access-Control-Expose-Headers", LinksHeader.HeaderName);
            headers.Add(LinksHeader.HeaderName, header.HeaderValues);
        }
    }
}