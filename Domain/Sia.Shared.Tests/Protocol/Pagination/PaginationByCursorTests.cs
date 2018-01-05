using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sia.Shared.Tests.Protocol.Pagination
{
    [TestClass]
    public class PaginationByCursorTests
    {
        [TestInitialize]
        public void ConfigureAutomapper()
            => Mapper.Initialize(configuration =>
                {
                    configuration.CreateMap<SimplePaginatableEntity, SimplePaginatableDto>();
                });

        private string GetValueByKey(IEnumerable<KeyValuePair<string, string>> tokens, string key)
            => tokens.FirstOrDefault(s => s.Key.Equals(key)).Value;

        [TestMethod]
        public async Task GetPageAsync_WithUninitializedPaginationByCursor_Returns50LatestResults()
        {
            var context = await SimplePaginatableContext
                .GetMockAsync(nameof(GetPageAsync_WithUninitializedPaginationByCursor_Returns50LatestResults));
            var objectUnderTest = new SimplePaginationByCursorRequest();

            var result = await objectUnderTest.GetFullyTypedResultAsync(context.SimplePaginatableEntities);          

            //Get Expected Range
            Assert.AreEqual(199, result.QueryResult[0].TestIndexedProperty, "Results index 0");
            Assert.AreEqual(150, result.QueryResult[49].TestIndexedProperty, "Results index 49");
            Assert.AreEqual(50, result.QueryResult.Count, "Results count");
            //Set expected LastResult
            Assert.AreEqual(150, result.LastResult.CursorIndex, "Last Result Value");
            //Set expected FirstResult
            Assert.AreEqual(199, result.FirstResult.CursorIndex, "First Result Value");

            var nextPage = result.NextPageLinkInfo;
            //Preserve cursor and sort order on next page
            Assert.AreEqual("desc", GetValueByKey(nextPage, nameof(objectUnderTest.SeekDirection)), $"Next Page {nameof(objectUnderTest.SeekDirection)}");
            Assert.AreEqual("desc", GetValueByKey(nextPage, nameof(objectUnderTest.SortOrder)), $"Next Page {nameof(objectUnderTest.SortOrder)}");
            //Use correct cursor value on next page
            Assert.AreEqual(150.ToString(), GetValueByKey(nextPage, nameof(objectUnderTest.CursorIndex)), $"Next Page {nameof(objectUnderTest.CursorIndex)}");

            var previousPage = result.PreviousPageLinkInfo;
            //Preserve sort order on previous page
            Assert.AreEqual("desc", GetValueByKey(previousPage, nameof(objectUnderTest.SortOrder)), $"Previous Page {nameof(objectUnderTest.SortOrder)}");
            //Reverse cursor direction on previous page
            Assert.AreEqual("asc", GetValueByKey(previousPage, nameof(objectUnderTest.SeekDirection)), $"Previous Page {nameof(objectUnderTest.SeekDirection)}");
            //Use correct cursor value on previous page
            Assert.AreEqual(199.ToString(), GetValueByKey(previousPage, nameof(objectUnderTest.CursorIndex)), $"Previous Page {nameof(objectUnderTest.CursorIndex)}");
        }

        [TestMethod]
        public async Task GetPageAsync_WithPaginationByCursorInitializedToPullFromMiddleOfRange_Returns50ExpectedResults()
        {
            var context = await SimplePaginatableContext
                .GetMockAsync(nameof(GetPageAsync_WithPaginationByCursorInitializedToPullFromMiddleOfRange_Returns50ExpectedResults));
            var objectUnderTest = new SimplePaginationByCursorRequest()
            {
                CursorIndex = 125,
                SortOrderBool = true, //asc
                SeekDirectionBool = false //desc
            };

            var result = await objectUnderTest.GetFullyTypedResultAsync(context.SimplePaginatableEntities);

            //Get Expected Range
            Assert.AreEqual(75, result.QueryResult[0].TestIndexedProperty, "Results index 0");
            Assert.AreEqual(124, result.QueryResult[49].TestIndexedProperty, "Results index 49");
            Assert.AreEqual(50, result.QueryResult.Count, "Results count");
            //Set expected LastResult
            Assert.AreEqual(124, result.LastResult.CursorIndex, "Last Result Value");
            //Set expected FirstResult
            Assert.AreEqual(75, result.FirstResult.CursorIndex, "First Result Value");

            var nextPage = result.NextPageLinkInfo;
            //Preserve sort order on next page
            Assert.AreEqual("asc", GetValueByKey(nextPage, nameof(objectUnderTest.SortOrder)), $"Next Page {nameof(objectUnderTest.SortOrder)}");
            //Preserve seek direction on next page
            Assert.AreEqual("desc", GetValueByKey(nextPage, nameof(objectUnderTest.SeekDirection)), $"Next Page {nameof(objectUnderTest.SeekDirection)}");
            //Use correct cursor value on next page
            Assert.AreEqual(124.ToString(), GetValueByKey(nextPage, nameof(objectUnderTest.CursorIndex)), $"Next Page {nameof(objectUnderTest.CursorIndex)}");

            var previousPage = result.PreviousPageLinkInfo;
            //Preserve sort order on previous page
            Assert.AreEqual("asc", GetValueByKey(previousPage, nameof(objectUnderTest.SortOrder)), $"Previous Page {nameof(objectUnderTest.SortOrder)}");
            //reverse seek direction on previous page
            Assert.AreEqual("asc", GetValueByKey(previousPage, nameof(objectUnderTest.SeekDirection)), $"Previous Page {nameof(objectUnderTest.SeekDirection)}");
            //Use correct cursor value on previous page
            Assert.AreEqual(75.ToString(), GetValueByKey(previousPage, nameof(objectUnderTest.CursorIndex)), $"Previous Page {nameof(objectUnderTest.CursorIndex)}");
        }
    }
}
