using System.Net;
using Leaf.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Leaf.Authorization
{
    [Authorize]
    [Route("auth")]
    [ServiceFilter(typeof(ObjectResultExceptionFilterAttribute))]
    public class AuthenticationController : Controller
    {
        public AuthenticationController(AuthenticationService authService, IApplicationLifetime appLifetime)
        {
            AuthService = authService;
            ApplicationLifetime = appLifetime;
        }

        private AuthenticationService AuthService { get; }
        private IApplicationLifetime ApplicationLifetime { get; }

        [Route("authenticate")]
        [HttpPost]
        [AllowAnonymous]
        public IActionResult RequestToken([FromBody] AuthenticationRequestModel request)
        {
            if (User.Identity.IsAuthenticated)
                return StatusCode((int) HttpStatusCode.BadRequest, new
                {
                    message = "이미 인증이 되어 있습니다."
                });

            return Ok(AuthService.Authenticate(request));
        }

        [Route("refresh")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult RefreshToken()
        {
            ShutdownSite();
            // User.Claims
            return new EmptyResult();
        }

        public void ShutdownSite()
        {
            ApplicationLifetime.StopApplication();
        }
    }
}