using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using KitNugs.Configuration;
using KitNugs.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KitNugs.Controllers
{
    [ApiController] //if we mark a controller with the [ApiController]  attribute, it will automatically trigger an HTTP 400 response if there is a model validation error.
    [Authorize]
    public class UserController : HelloControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly IExceptionHandler _exceptionHandler;
        private readonly ITokenHandler _tokenHandler;

        public UserController(
            ILogger<UserController> logger,
            IServiceConfiguration configuration,
            IUserService userService,
            IExceptionHandler exceptionHandler,
            ITokenHandler tokenHandler)
        {
            _logger = logger;
            _userService = userService;
            _exceptionHandler = exceptionHandler;
            _tokenHandler = tokenHandler;
        }

        public override async Task<IActionResult> Users([FromBody] UserResponse body)
        {

            try
            {
                var token = await _tokenHandler.getJwt(HttpContext);
                var customerId = _tokenHandler.ExtractTenantIdFromToken(token);
                await _userService.CreateUser(customerId, body);
                return Ok(body);
            }
            catch (Exception e)
            {
                return _exceptionHandler.HandleException(e);
            }
        }

        public override async Task<IActionResult> UsersAll([FromQuery] int? page, [FromQuery] int? limit)
        {
            try
            {
                var token = await _tokenHandler.getJwt(HttpContext);
                var tenantId = _tokenHandler.ExtractTenantIdFromToken(token);

                IList<UserResponse> users = await _userService.GetUsersForTenant(tenantId, page, limit);
                return base.Ok(users);
            }
            catch (Exception e)
            {
                return _exceptionHandler.HandleException(e);
            }
        }


    }
}
