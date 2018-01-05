using Sia.Shared.Protocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Sia.Shared.Protocol.Pagination;

namespace Sia.Shared.Tests.Protocol.Pagination
{
    public class SimplePaginatableEntity
    {
        public long Id { get; set; }
        public long TestIndexedProperty { get; set; }
        public static IEnumerable<SimplePaginatableEntity> GetEntities(long numberToGenerate, long startingValue = 0)
        {
            for (int i = 0; i < numberToGenerate; i++)
            {
                yield return new SimplePaginatableEntity()
                {
                    TestIndexedProperty = startingValue + i
                };
            }
        }
    }

    public class SimplePaginatableDto
    {
        public long TestIndexedProperty { get; set; }
    }

    public class SimplePaginatableContext : DbContext
    {
        public SimplePaginatableContext(string instance)
            : base(new DbContextOptionsBuilder<SimplePaginatableContext>()
                    .UseInMemoryDatabase(instance)
                    .Options) { }
        public static async Task<SimplePaginatableContext> GetMockAsync(
            string instance,
            long numberOfRecords = 200, 
            long startingValue = 0)
        {
            var context = new SimplePaginatableContext(instance);
            context.SimplePaginatableEntities.AddRange(
                SimplePaginatableEntity.GetEntities(numberOfRecords, startingValue)
            );
            await context.SaveChangesAsync();
            return context;
        }

        public DbSet<SimplePaginatableEntity> SimplePaginatableEntities { get; set; }
    }

    public class SimplePaginationByCursorRequest
        : PaginationByCursorRequest<SimplePaginatableEntity, SimplePaginatableDto, SimplePaginationCursor>
    {
        public long CursorIndex { get; set; }

        public override IPaginationByCursorSelectors<SimplePaginatableEntity, SimplePaginatableDto, SimplePaginationCursor> Selectors()
            => _selectors;
        private SimplePaginationByCursorSelectors _selectors = new SimplePaginationByCursorSelectors();
        public override SimplePaginationCursor GetCursorValue() => new SimplePaginationCursor()
            {
                CursorIndex = CursorIndex
            };
    }

    public class SimplePaginationByCursorSelectors
        : PaginationByCursorSelectors<SimplePaginatableEntity, SimplePaginatableDto, SimplePaginationCursor>
    {
        public override Expression<Func<SimplePaginatableEntity, SimplePaginationCursor>> CursorSelector
            => (entity) => new SimplePaginationCursor()
            {
                CursorIndex = entity.TestIndexedProperty
            };

        public override Func<SimplePaginatableDto, SimplePaginationCursor> DtoValueSelector
            => (dto) => new SimplePaginationCursor()
            {
                CursorIndex = dto.TestIndexedProperty
            };
    }

    public class SimplePaginationCursor
        : IComparable<SimplePaginationCursor>,
        IPaginationCursor<SimplePaginationCursor>
    {
        public long CursorIndex { get; set; }

        public int CompareTo(SimplePaginationCursor other)
            => CursorIndex.CompareTo(other.CursorIndex);
        public bool IsInitialized()
            => !CursorIndex.Equals(default(long));

        public IEnumerable<KeyValuePair<string, string>> SerializationTokens()
        {
            yield return new KeyValuePair<string, string>(nameof(CursorIndex), CursorIndex.ToString());
        }
    }
}
