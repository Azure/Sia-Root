using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sia.Shared.Authentication;
using Sia.Shared.Validation.Filters;
using Sia.Shared.Protocol;
using System.Linq;

namespace Sia.Shared.Controllers
{
    [Return400BadRequestWhenModelStateInvalid]
    //[Authorize()]
    public abstract class BaseController : Controller
    {
        protected readonly IMediator _mediator;
        protected readonly AzureActiveDirectoryAuthenticationInfo _authConfig;
        protected readonly IUrlHelper _urlHelper;

        protected AuthenticatedUserContext _authContext => new AuthenticatedUserContext(User, HttpContext.Session, _authConfig);

        protected BaseController(IMediator mediator, 
            AzureActiveDirectoryAuthenticationInfo authConfig,
            IUrlHelper urlHelper)
        {
            _mediator = mediator;
            _authConfig = authConfig;
            _urlHelper = urlHelper;
        }
    }
}
