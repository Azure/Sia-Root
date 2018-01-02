using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sia.Shared.Protocol;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sia.Shared.Protocol
{
    public interface IPaginationMetadata
    {
        IEnumerable<KeyValuePair<string, string>> PaginationHeaderValues();
        bool NextPageExists { get; }
        bool PreviousPageExists { get; }
        IEnumerable<KeyValuePair<string, string>> PreviousPageLinkInfo { get; }
        IEnumerable<KeyValuePair<string, string>> NextPageLinkInfo { get; }
    }

    public interface IPaginator<T>
    {
        IQueryable<T> Paginate(IQueryable<T> source);
    }

    public interface ITrackingPaginator<TSource, TDestination>
        : IPaginator<TSource>
    { 
        List<TDestination> QueryResult { get; set; }
        long TotalRecords { get; set; }
    }

    public abstract class PaginationMetadata<T>
        : IPaginationMetadata,
        IPaginator<T>
    {
        public int MaxPageSize { get; set; } = 50;
        public long TotalRecords { get; set; }
        public long TotalPages => (TotalRecords / MaxPageSize) + (TotalRecords % MaxPageSize > 0 ? 1 : 0);
        public abstract IEnumerable<KeyValuePair<string, string>> PreviousPageLinkInfo { get; }
        public abstract IEnumerable<KeyValuePair<string, string>> NextPageLinkInfo { get; }
        public abstract bool NextPageExists { get; }
        public abstract bool PreviousPageExists { get; }

        public abstract IQueryable<T> Paginate(IQueryable<T> source);

        public virtual IEnumerable<KeyValuePair<string, string>> PaginationHeaderValues()
        {
            yield return new KeyValuePair<string, string>(nameof(MaxPageSize), MaxPageSize.ToString());
            yield return new KeyValuePair<string, string>(nameof(TotalRecords), TotalRecords.ToString());
            yield return new KeyValuePair<string, string>(nameof(TotalPages), TotalPages.ToString());
        }
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
            var resultsQuery = source
                .WithPagination(pagination)
                .ProjectTo<TDestination>()
                .ToListAsync();

            var countQuery = source.CountAsync();

            pagination.TotalRecords = await countQuery;
            pagination.QueryResult = await resultsQuery;

            return pagination.QueryResult;
        }
    }
}
