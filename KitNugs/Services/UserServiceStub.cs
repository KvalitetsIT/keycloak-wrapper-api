using KitNugs.Configuration;
using KitNugs.Controllers;
using KitNugs.Services.Model;

namespace KitNugs.Services
{
    public class UserServiceStub : IUserService
    {
        private readonly string _configurationValue;
        private readonly ILogger<UserServiceStub> _logger;
        private static IList<UserResponse> _storage = new List<UserResponse>();

        public UserServiceStub(IServiceConfiguration configuration, ILogger<UserServiceStub> logger)
        {
            _configurationValue = configuration.GetConfigurationValue(ConfigurationVariables.AllowedAudience);
            _logger = logger;
        }

        public async Task CreateUser(UserResponse userToCreate)
        {
            _storage.Add(userToCreate);
        }

        public async Task<IList<UserResponse>> GetUsers()
        {
            return _storage;
        }
    }
}
