using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sia.Shared.Protocol.Pagination
{
    public class PaginationByFixedPageSizeRequest<TSource, TDestination>
        : IPaginationRequest<TSource, TDestination>
    {
        public int MaxPageSize { get; set; } = 50;
        public int PageNumber { get; set; } = 1;
        public async Task<IPaginationResultMetadata<TDestination>> GetResultAsync(IQueryable<TSource> source)
        {
            var query = source
                .Skip((PageNumber - 1) * MaxPageSize)
                .Take(MaxPageSize)
                .ProjectTo<TDestination>()
                .ToListAsync();

            var countQuery = source.CountAsync();

            var queryResult = await query;
            var totalRecords = await countQuery;

            return new PaginationByFixedPageSizeResult<TSource, TDestination>(this, queryResult, totalRecords);
        }
    }
}
