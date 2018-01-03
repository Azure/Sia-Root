﻿using AutoMapper.QueryableExtensions;
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
        where TCursor : IComparable<TCursor>
    {
        public TCursor CursorValue { get; set; }
        public int MaxPageSize { get; } = 50;
        public bool CursorDirectionBool = false;
        public bool SortOrderBool = false;

        /// <summary>
        /// If "asc" / true, result values will be greater than or equal to CursorValue,
        /// if "desc" / false, result values will be less than or equal to CursorValue
        /// </summary>
        public string CursorDirection
        {
            get => TranslateFromDirectionalBool(CursorDirectionBool);
            set => CursorDirectionBool = TranslateToDirectionalBool(value);
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
            var filteredResults = source
                .Where(ValidRecord);
            var orderedResults = SortOrderBool
                ? filteredResults.OrderBy(DataValueSelector)
                : filteredResults.OrderByDescending(DataValueSelector);
            var query = orderedResults
                .Take(MaxPageSize)
                .ProjectTo<TDestination>()
                .ToListAsync();

            var countQuery = source.CountAsync();

            var queryResult = await query;
            var totalRecords = await countQuery;

            return new PaginationByCursorResult<TSource, TDestination, TCursor>(this, totalRecords, queryResult);
        }

        public async Task<IPaginationResultMetadata<TDestination>> GetResultAsync(IQueryable<TSource> source)
            => await GetFullyTypedResultAsync(source);

        protected Expression<Func<TSource, bool>> ValidRecord
            => (record)
            => CursorDirectionBool
                ? CompiledCursorSelector(record).CompareTo(CursorValue) <= 0
                //Return values are equal to or greater than CursorValue
                : CompiledCursorSelector(record).CompareTo(CursorValue) >= 0;
        //Return values are equal to or less than CursorValue

        public Func<TSource, TCursor> CompiledCursorSelector
        {
            get
            {
                if (_compiledCursorSelector is null)
                {
                    _compiledCursorSelector = DataValueSelector.Compile();
                }
                return _compiledCursorSelector;
            }
        }

        private Func<TSource, TCursor> _compiledCursorSelector;
        protected abstract Expression<Func<TSource, TCursor>> DataValueSelector { get; }
        public abstract Func<TDestination, TCursor> DtoValueSelector { get; }
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