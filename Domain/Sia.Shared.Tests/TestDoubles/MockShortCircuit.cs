﻿using Sia.Shared.Extensions.Mediatr;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Sia.Gateway.Tests.TestDoubles
{
    internal class MockShortCircuit : HandlerShortCircuit<StubRequest, string>
    {
        private readonly bool _shouldRequestContinue;
        public MockShortCircuit(bool shouldRequestContinue) : base(null)
        {
            _shouldRequestContinue = shouldRequestContinue;
        }

        public override Task<string> GenerateMockAsync(StubRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult("Next was not called");
        }

        public override bool ShouldRequestContinue(IConfigurationRoot config)
        {
            return _shouldRequestContinue;
        }
    }

    public class StubRequest : IRequest<string>
    {
        
    }
}
