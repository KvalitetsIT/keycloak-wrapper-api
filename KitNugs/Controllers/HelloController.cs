using KitNugs.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KitNugs.Controllers
{
    [Authorize]
    public class HelloController : HelloControllerBase
    {
        private readonly ILogger<HelloController> _logger;
        private readonly IUserService _userService;

        public HelloController(ILogger<HelloController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public override async Task<UserResponse> Users([FromBody] UserResponse body)
        {
            await _userService.CreateUser(body);
            return body;
        }

        public override async Task<ICollection<UserResponse>> UsersAll([FromQuery] int? page, [FromQuery] int? limit)
        {
            return await _userService.GetUsers();
        }
    }
}
