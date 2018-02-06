using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sia.Shared.Tests.TestDoubles;

namespace Sia.Shared.Tests.Controllers
{

    [TestClass]
    public class BaseControllerTests
    {
        [TestMethod]
        public async Task Return_OkResultObject_OkIfFound_Input_Is_Not_Null()
        {
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK);
            var mockMediator = new Mock<IMediator>();
            var mockConfig = new Mock<DummyAzureActiveDirectoryAuthenticationInfo>();
            var mockUrlHelper = new Mock<IUrlHelper>();
            var controller = new StubController(mockMediator.Object, mockConfig.Object, mockUrlHelper.Object);
            var expectedResult = new OkObjectResult("not null");

            var result = controller.OkIfFound(mockResponse);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.GetType(), expectedResult.GetType());
        }

        [TestMethod]
        public async Task ReturnNotFoundResultObject_OkIfFound_Input_Is_Null()
        {
            var mockMediator = new Mock<IMediator>();
            var mockConfig = new Mock<DummyAzureActiveDirectoryAuthenticationInfo>();
            var mockUrlHelper = new Mock<IUrlHelper>();
            var controller = new StubController(mockMediator.Object, mockConfig.Object, mockUrlHelper.Object);
            var expectedResult = new NotFoundResult();

            var result = controller.OkIfFound("null");
            
            Assert.IsNotNull(result);
            Assert.AreEqual(result.GetType(), expectedResult.GetType());
        }
    }
}
