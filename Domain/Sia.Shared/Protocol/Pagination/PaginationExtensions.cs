using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Sia.Shared.Protocol;
using Sia.Shared.Protocol.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Linq
{
    public static class PaginationExtensions
    {
        public static Task<IPaginationResultMetadata<TDestination>> GetPaginatedResultAsync<TSource, TDestination>(
            this IQueryable<TSource> source,
            IPaginationRequest<TSource, TDestination> pagination
        ) => pagination.GetResultAsync(source);
    }
}
