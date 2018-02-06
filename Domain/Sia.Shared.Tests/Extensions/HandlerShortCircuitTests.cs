using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sia.Gateway.Tests.TestDoubles;

namespace Sia.Shared.Tests.Extensions
{
    [TestClass]
    public class HandlerShortCircuitTests
    {
        [TestMethod]
        public async Task Handle_Should_Return_Next_If_ShouldRequestContinue_Is_True()
        {
            Task<string> ReturnMockNext() => Task.FromResult("Next result");
            var mockShortCircuit = new MockShortCircuit(shouldRequestContinue: true);

            var result = await mockShortCircuit.Handle(null, new CancellationToken(), ReturnMockNext);

            Assert.AreEqual(result, "Next result");
        }

        [TestMethod]
        public async Task Handle_Should_Return_Mock_If_ShouldRequestContinue_Is_False()
        {
            var mockShortCircuit = new MockShortCircuit(shouldRequestContinue: false);

            var result = await mockShortCircuit.Handle(null, new CancellationToken(), null);

            Assert.AreEqual(result, "Next was not called");
        }
    }
}
