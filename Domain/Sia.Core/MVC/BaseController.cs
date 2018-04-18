using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sia.Core.Authentication;
using Sia.Core.Data;
using Sia.Core.Protocol;
using Sia.Core.Validation;
using Sia.Core.Validation.Filters;

namespace Sia.Core.Controllers
{
    [Return400BadRequestWhenModelStateInvalid]
    [Authorize()]
    public abstract class BaseController : Controller
    {
        protected readonly IMediator _mediator;
        protected readonly AzureActiveDirectoryAuthenticationInfo _authConfig;
        protected readonly IUrlHelper _urlHelper;
        protected ILogger logger { get; set; }

        protected AuthenticatedUserContext AuthContext => new AuthenticatedUserContext(User, HttpContext.Session, _authConfig);

        protected BaseController(IMediator mediator, 
            AzureActiveDirectoryAuthenticationInfo authConfig,
            IUrlHelper urlHelper)
        {
            _mediator = mediator;
            _authConfig = authConfig;
            _urlHelper = urlHelper;
        }

        public static IActionResult ServerError()
            => new StatusCodeResult(500);

        public IActionResult OkIfFound<TResponse>(TResponse response, ILinksHeader links = null)
        where TResponse : class
        {
            if (response == null)
            {
                return NotFound();
            }

            if(links != null)
            {
                Response.Headers.AddLinksHeader(links);
            }
            return Ok(response);
        }

        public IActionResult CreatedIfExists<TResponse>(
            TResponse response,
            Func<TResponse, Uri> getRetrievalRoute,
            Func<TResponse, ILinksHeader> getLinks
        )
        {
            if(response == null)
            {
                // POST requests should result in a created record
                // if no record is created, we have made a mistake.
                return ServerError();
            }

            var links = getLinks(response);
            if(links != null)
            {
                Response.Headers.AddLinksHeader(links);
            }

            var retrievalRoute = getRetrievalRoute(response);
            return Created(retrievalRoute, response);
        }
    }
}
