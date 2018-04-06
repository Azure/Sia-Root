using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Sia.Core.Data
{
    
    public interface IJsonDataString
    {
        string Data { get; set; }
    }

    public interface IJsonDataObject
    {
        object Data { get; set; }
    }

    public class ResolveJsonToString<TSource, TDestination>
        : IValueResolver<TSource, TDestination, string>
        where TSource: IJsonDataObject
        where TDestination: IJsonDataString
    {
        public string Resolve(TSource source, TDestination destination, string destMember, ResolutionContext context)
            => source?.Data is null ? null : JsonConvert.SerializeObject(source.Data);
    }

    public class ResolveStringToJson<TSource, TDestination>
        : IValueResolver<TSource, TDestination, object>
        where TSource : IJsonDataString
        where TDestination : IJsonDataObject
    {
        public object Resolve(TSource source, TDestination destination, object destMember, ResolutionContext context)
            => source?.Data is null ? null : JsonConvert.DeserializeObject(source.Data);
    }
}
