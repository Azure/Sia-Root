using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace Sia.Shared.Protocol
{
    public class LinksHeader
    {
        private PaginationMetadata _metadata;
        private IUrlHelper _urlHelper;
        private string _routeName;

        public LinksHeader(PaginationMetadata metadata, IUrlHelper urlHelper, string routeName)
        {
            _metadata = metadata;
            _urlHelper = urlHelper;
            _routeName = routeName;
        }

        public const string HeaderName = "links";
        public virtual StringValues HeaderValues =>
            _baseHeaderValues;

        private StringValues _baseHeaderValues => JsonConvert.SerializeObject(new
        {
            PageNumber = _metadata.PageNumber,
            PageSize = _metadata.PageSize,
            TotalRecords = _metadata.TotalRecords,
            TotalPages = _metadata.TotalPages,
            NextPageLink = _metadata.NextPageExists ? _urlHelper.Action(_routeName, _metadata.NextPageLinkInfo) : null,
            PrevPageLink = _metadata.PreviousPageExists ? _urlHelper.Action(_routeName, _metadata.PreviousPageLinkInfo) : null
        });
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