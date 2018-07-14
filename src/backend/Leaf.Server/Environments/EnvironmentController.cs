using System.Net;
using Leaf.Authorization;
using Leaf.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Leaf.Environments
{
    [Route("env")]
    [Authorize]
    [ServiceFilter(typeof(ObjectResultExceptionFilterAttribute))]
    public class EnvironmentController : Controller
    {
        public EnvironmentController(IEnvironmentProvider provider, JwtTokenProvider tokenProvider)
        {
            EnvProvider = provider;
            TokenProvider = tokenProvider;
        }

        private IEnvironmentProvider EnvProvider { get; }
        private JwtTokenProvider TokenProvider { get; }

        [Route("global")]
        [HttpGet]
        [AllowAnonymous]
        [ServiceFilter(typeof(ObjectResultExceptionFilterAttribute))]
        public IActionResult GlobalEnvironments()
        {
            return Ok(EnvProvider.GetGlobalEnvironments<GlobalEnvironments>());
        }

        [Route("login")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult LoginEnvironments()
        {
            return Ok(EnvProvider.GetLoginEnvironments<LoginEnvironments>());
        }

        [Route("main")]
        [HttpGet]
        public IActionResult MainEnvironments()
        {
            if (Request.Headers.TryGetValue("Authority", out var acc))
            {
                if (!TokenProvider.ValidateJwtSecurityToken(acc))
                    return StatusCode((int) HttpStatusCode.Unauthorized, new
                    {
                        message = "Invalid AuthorizationGroup Token"
                    });
            }
            else
            {
                return StatusCode((int) HttpStatusCode.Unauthorized, new
                {
                    message = "Invalid AuthorizationGroup Token"
                });
            }

            return Ok(EnvProvider.GetLoginEnvironments<LoginEnvironments>());
        }
    }
}