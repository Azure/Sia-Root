using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sia.Shared.Authentication;
using Sia.Shared.Protocol;
using Sia.Shared.Validation.Filters;

namespace Sia.Shared.Controllers
{
    [Return400BadRequestWhenModelStateInvalid]
    [Authorize()]
    public abstract class BaseController : Controller
    {
        protected readonly IMediator _mediator;
        protected readonly AzureActiveDirectoryAuthenticationInfo _authConfig;
        protected readonly IUrlHelper _urlHelper;

        protected AuthenticatedUserContext authContext => new AuthenticatedUserContext(User, HttpContext.Session, _authConfig);

        protected BaseController(IMediator mediator, 
            AzureActiveDirectoryAuthenticationInfo authConfig,
            IUrlHelper urlHelper)
        {
            _mediator = mediator;
            _authConfig = authConfig;
            _urlHelper = urlHelper;
        }

        public IActionResult OkIfFound<TResponse>(TResponse response)
        where TResponse : class
        {
            if (response == null || response == "null")
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }
        }

        public IActionResult OkIfAny<TResponse>(IEnumerable<TResponse> response)
        {
            if (response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }
        }
    }
}
