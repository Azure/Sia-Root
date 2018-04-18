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