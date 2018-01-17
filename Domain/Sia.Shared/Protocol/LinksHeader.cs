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
        private long _incidentId;

        public LinksHeader(PaginationMetadata metadata, IUrlHelper urlHelper, string routeName, long incidentId)
        {
            _metadata = metadata;
            _urlHelper = urlHelper;
            _routeName = routeName;
            _incidentId = incidentId;
        }

        public const string HeaderName = "links";
        public string HeaderJson => getHeaderValues();

        protected virtual IEnumerable<KeyValuePair<string, string>> _headerValues()
        {
            yield return new KeyValuePair<string, string>("PageNumber", _metadata.PageNumber.ToString());
            yield return new KeyValuePair<string, string>("PageSize", _metadata.PageSize.ToString());
            yield return new KeyValuePair<string, string>("TotalRecords", _metadata.TotalRecords.ToString());
            yield return new KeyValuePair<string, string>("TotalPages", _metadata.TotalPages.ToString());         
        }

        protected string getHeaderValues() {
            string returnJsonHeader;
            string paginationMetadata = FormatJson(_getPaginationValues());
            string paginationLinks = FormatJson(_getPaginationLinks());
            string singleOps = FormatJson(_getSingleOps());

            

            return returnJsonHeader;
        }

        //IDictoinary<string, object> is the value type in Ok();
        protected IEnumerable<KeyValuePair<string, string>> _getPaginationLinks() {
            if (_metadata.NextPageExists) yield return new KeyValuePair<string,string>("NextPageLink", _nextPageLink);
            if (_metadata.PreviousPageExists) yield return new KeyValuePair<string,string>("PrevPageLink", _previousPageLink);
        }

        protected IEnumerable<KeyValuePair<string, string>> _getPaginationValues() {
            yield return new KeyValuePair<string, string>("PageNumber", _metadata.PageNumber.ToString());
            yield return new KeyValuePair<string, string>("PageSize", _metadata.PageSize.ToString());
            yield return new KeyValuePair<string, string>("TotalRecords", _metadata.TotalRecords.ToString());
            yield return new KeyValuePair<string, string>("TotalPages", _metadata.TotalPages.ToString());
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
        protected string JsonTokenFormat(KeyValuePair<string, string> token)
            => $"\"{token.Key}\":\"{token.Value}\"";

        protected string FormatUrl(IEnumerable<KeyValuePair<string, string>> tokens)
            => "/?" + string.Join("&", tokens.Select(UrlTokenFormat));

        protected string FormatJson(IEnumerable<KeyValuePair<string, string>> tokens)
            => "{" + string.Join(",", tokens.Select(JsonTokenFormat)) + "}";
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
            headers.Add(LinksHeader.HeaderName, header.HeaderJson);
        }
    }
}