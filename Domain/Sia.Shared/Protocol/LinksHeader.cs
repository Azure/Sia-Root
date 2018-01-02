﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Sia.Shared.Validation;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sia.Shared.Protocol
{
    public class LinksHeader
    {
        private IPaginationMetadata _metadata;
        private IUrlHelper _urlHelper;
        private string _routeName;

        public LinksHeader(IPaginationMetadata metadata, IUrlHelper urlHelper, string routeName)
        {
            _metadata = ThrowIf.Null(metadata, nameof(metadata));
            _urlHelper = ThrowIf.Null(urlHelper, nameof(urlHelper));
            _routeName = ThrowIf.NullOrWhiteSpace(routeName, nameof(routeName));
        }

        public const string HeaderName = "links";
        public string HeaderJson => FormatJson(_headerValues());

        protected virtual IEnumerable<KeyValuePair<string, string>> _headerValues()
        {
            foreach (var headerValue in _metadata.PaginationHeaderValues())
            {
                yield return headerValue;
            }
            if (_metadata.NextPageExists) yield return new KeyValuePair<string, string>("NextPageLink", _nextPageLink);
            if (_metadata.PreviousPageExists) yield return new KeyValuePair<string, string>("PrevPageLink", _previousPageLink);
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