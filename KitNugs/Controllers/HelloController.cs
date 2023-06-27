using KitNugs.Services;
using Microsoft.AspNetCore.Mvc;

namespace KitNugs.Controllers
{
    public class HelloController : HelloControllerBase
    {
        private readonly ILogger<HelloController> _logger;
        private readonly IHelloService _helloService;
        private static List<UserResponse> _storage = new List<UserResponse>();

        public HelloController(ILogger<HelloController> logger, IHelloService helloService)
        {
            _logger = logger;
            _helloService = helloService;
        }

        public override async Task<UserResponse> Users([FromBody] UserResponse body)
        {
            _storage.Add(body);
            return await Task.FromResult(body);
        }

        public override async Task<ICollection<UserResponse>> UsersAll([FromQuery] int? page, [FromQuery] int? limit)
        {
            return await Task.FromResult(_storage);
        }
    }
}
