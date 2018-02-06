using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;  
using Sia.Shared.Authentication;
using Sia.Shared.Controllers;

namespace Sia.Shared.Tests.TestDoubles
{

    public class StubController : BaseController
    {
        protected new readonly IMediator _mediator;
        protected new readonly DummyAzureActiveDirectoryAuthenticationInfo _authConfig;
        protected new readonly IUrlHelper _urlHelper;

        public StubController(IMediator mediator,
            DummyAzureActiveDirectoryAuthenticationInfo authConfig,
            IUrlHelper urlHelper) : base(mediator, authConfig, urlHelper)
        {
            _mediator = mediator;
            _authConfig = authConfig;
            _urlHelper = urlHelper;
        }
    }
}
