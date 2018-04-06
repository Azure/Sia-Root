using Sia.Core.Extensions.Mediatr;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Sia.Gateway.Tests.TestDoubles
{
    internal class MockShortCircuit : HandlerShortCircuit<IRequest<string>, string, string>
    {
        private readonly bool _shouldRequestContinue;
        public MockShortCircuit(bool shouldRequestContinue) : base(null)
        {
            _shouldRequestContinue = shouldRequestContinue;
        }

        public override Task<string> GenerateMockAsync(IRequest<string> request, CancellationToken cancellationToken)
        {
            return Task.FromResult("Next was not called");
        }

        public override bool ShouldRequestContinue(string config)
        {
            return _shouldRequestContinue;
        }
    }
}
