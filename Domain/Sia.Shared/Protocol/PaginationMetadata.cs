using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Sia.Shared.Protocol;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sia.Shared.Protocol
{
    public interface IPaginationLinks
    {
        StringValues HeaderValues(IUrlHelper urlHelper, string routeName);
    }

    public interface IPaginator<T>
    {
        IQueryable<T> Paginate(IQueryable<T> source);
    }

    public interface ITrackingPaginator<TSource, TDestination>
        : IPaginator<TSource>
    { 
        List<TDestination> QueryResult { get; set; }
    }

    public abstract class PaginationMetadata<T>
        : IPaginationLinks,
        IPaginator<T>
    {
        public int MaxPageSize { get; set; } = 50;
        public long TotalRecords { get; set; }
        public long TotalPages => (TotalRecords / MaxPageSize) + (TotalRecords % MaxPageSize > 0 ? 1 : 0);
        public StringValues PreviousPageLinkInfo
            => PreviousPageExists
            ? StringValues.Concat(CommonLinkValues(), PreviousPageLinkValues)
            : StringValues.Empty;

        public StringValues NextPageLinkInfo
            => NextPageExists
            ? StringValues.Concat(CommonLinkValues(), NextPageLinkValues)
            : StringValues.Empty;

        protected virtual StringValues CommonLinkValues()
            => JsonConvert.SerializeObject(new
            {
                MaxPageSize = MaxPageSize
            });

        protected abstract StringValues NextPageLinkValues { get; }
        protected abstract StringValues PreviousPageLinkValues { get; }
        public abstract bool PreviousPageExists { get; }
        public abstract bool NextPageExists { get; }
        public virtual IQueryable<T> Paginate(IQueryable<T> source)
        {
            TotalRecords = source.LongCount();
            return ImplementPagination(source);
        }

        protected abstract IQueryable<T> ImplementPagination(IQueryable<T> source);

        public virtual StringValues HeaderValues(IUrlHelper urlHelper, string routeName)
             => JsonConvert.SerializeObject(new
             {
                 MaxPageSize = MaxPageSize,
                 TotalRecords = TotalRecords,
                 TotalPages = TotalPages,
                 NextPageLink = NextPageExists ? urlHelper.Action(routeName, NextPageLinkInfo) : null,
                 PrevPageLink = PreviousPageExists ? urlHelper.Action(routeName, PreviousPageLinkInfo) : null
             });
    }
}

namespace System.Linq
{
    public static class PaginationExtensions
    {
        public static IQueryable<T> WithPagination<T>(
            this IQueryable<T> source, 
            IPaginator<T> pagination
        ) => pagination.Paginate(source);

        public static async Task<List<TDestination>> GetPageAsync<TSource, TDestination>(
            this IQueryable<TSource> source,
            ITrackingPaginator<TSource, TDestination> pagination
        )
        {
            pagination.QueryResult = await source
                .WithPagination(pagination)
                .ProjectTo<TDestination>()
                .ToListAsync();

            return pagination.QueryResult;
        }
    }
}
