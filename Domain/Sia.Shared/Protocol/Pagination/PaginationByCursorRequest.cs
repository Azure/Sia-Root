using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sia.Shared.Protocol.Pagination
{
    public abstract class PaginationByCursorRequest<TSource, TDestination, TCursor>
        : IPaginationRequest<TSource, TDestination>
        where TCursor : IComparable<TCursor>, IPaginationCursor<TCursor>
    {
        public abstract TCursor GetCursorValue();
        public int MaxPageSize { get; set; } = 50;
        public bool SeekDirectionBool { get; set; } = false;
        public bool SortOrderBool { get; set; } = false;

        /// <summary>
        /// If "asc" / true, result values will be greater than or equal to CursorValue,
        /// if "desc" / false, result values will be less than or equal to CursorValue
        /// </summary>
        public string SeekDirection
        {
            get => TranslateFromDirectionalBool(SeekDirectionBool);
            set => SeekDirectionBool = TranslateToDirectionalBool(value);
        }

        /// <summary>
        /// If "asc" / true, next page values will be greater than result values,
        /// if "desc" / false, next page values will be less than result values.
        /// Vice versa for previous page values.
        /// Also determines sort order on the result set.
        /// </summary>
        public string SortOrder
        {
            get => TranslateFromDirectionalBool(SortOrderBool);
            set => SortOrderBool = TranslateToDirectionalBool(value);
        }



        public async Task<PaginationByCursorResult<TSource, TDestination, TCursor>> GetFullyTypedResultAsync(IQueryable<TSource> source)
        {
            var filteredRecords = GetCursorValue().IsInitialized()
                ? source
                    .Where(ValidRecord)
                : source;

            var recordsInSeekOrder = SeekDirectionBool
                ? filteredRecords.OrderBy(Selectors().CursorSelector)
                : filteredRecords.OrderByDescending(Selectors().CursorSelector);

            var resultSet = recordsInSeekOrder
                .Take(MaxPageSize);

            var resultsInSortOrder = SortOrderBool
                ? resultSet.OrderBy(Selectors().CursorSelector)
                : resultSet.OrderByDescending(Selectors().CursorSelector);

            var resultsQuery = resultsInSortOrder
                .ProjectTo<TDestination>()
                .ToListAsync();

            var countQuery = source.CountAsync();

            var queryResult = await resultsQuery;
            var totalRecords = await countQuery;

            return new PaginationByCursorResult<TSource, TDestination, TCursor>(this, totalRecords, queryResult);
        }

        public async Task<IPaginationResultMetadata<TDestination>> GetResultAsync(IQueryable<TSource> source)
            => await GetFullyTypedResultAsync(source);

        protected Expression<Func<TSource, bool>> ValidRecord
            => (record)
            => SeekDirectionBool
                ? Selectors().CompiledCursorSelector(record).CompareTo(GetCursorValue()) > 0
                //Return values are greater than CursorValue
                : Selectors().CompiledCursorSelector(record).CompareTo(GetCursorValue()) < 0;
                //Return values are less than CursorValue

        public abstract IPaginationByCursorSelectors<TSource, TDestination, TCursor> Selectors();
        public string TranslateFromDirectionalBool(bool value)
            => value
                ? "asc"
                : "desc";
        public bool TranslateToDirectionalBool(string value)
            => value.Equals("asc")
                ? true
                : false;

    }
}
