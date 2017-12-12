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
            var objectUnderTest = new SimplePaginationByCursor();

            /*var valueProvider = new QueryStringValueProvider(
                    BindingSource.Query,
                    null,
                    CultureInfo.InvariantCulture
                );*/


            var result = await context
                .SimplePaginatableEntities
                .GetPageAsync(objectUnderTest);
            

            //Get Expected Range
            Assert.AreEqual(199, result[0].CursorTarget);
            Assert.AreEqual(150, result[49].CursorTarget);
            //Set expected final value
            Assert.AreEqual(150, objectUnderTest.FinalValue);
            //No previous page
            Assert.IsFalse(objectUnderTest.PreviousPageExists);
            //Next page exists
            Assert.IsTrue(objectUnderTest.NextPageExists);

            //Preserve cursor and sort order on next page
            Assert.AreEqual("CursorTarget=desc", objectUnderTest.NextPageLinkInfo.FirstOrDefault(s => s.Contains("CursorTarget")));
        }
    }
}
