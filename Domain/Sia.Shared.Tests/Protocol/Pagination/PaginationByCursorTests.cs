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

        [TestMethod]
        public async Task GetPageAsync_WithUnititializedPaginationByCursor_Returns50LatestResults()
        {
            var context = await SimplePaginatableContext
                .GetMockAsync(nameof(GetPageAsync_WithUnititializedPaginationByCursor_Returns50LatestResults));
            var objectUnderTest = new SimplePaginationByCursorRequest();

            var result = await objectUnderTest.GetFullyTypedResultAsync(context.SimplePaginatableEntities);          

            //Get Expected Range
            Assert.AreEqual(199, result.QueryResult[0].TestIndexedProperty);
            Assert.AreEqual(150, result.QueryResult[49].TestIndexedProperty);
            //Set expected final value
            Assert.AreEqual(150, result.LastResult);
            //No previous page
            Assert.IsFalse(result.PreviousPageExists);
            //Next page exists
            Assert.IsTrue(result.NextPageExists);

            //Preserve cursor and sort order on next page
            Assert.AreEqual("desc", result.NextPageLinkInfo.FirstOrDefault(s => s.Key.Equals("CursorDirection")).Value);
            Assert.AreEqual("desc", result.NextPageLinkInfo.FirstOrDefault(s => s.Key.Equals("SortOrder")).Value);

        }
    }
}
