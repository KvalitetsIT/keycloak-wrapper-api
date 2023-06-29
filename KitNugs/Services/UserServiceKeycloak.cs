using FS.Keycloak.RestApiClient.Api;
using FS.Keycloak.RestApiClient.Client;
using FS.Keycloak.RestApiClient.Model;
using KitNugs.Configuration;
using KitNugs.Controllers;
using KitNugs.Services.Model;

namespace KitNugs.Services
{
    public class UserServiceKeycloak : IUserService
    {
        private readonly string _realm;
        private readonly ILogger<UserServiceStub> _logger;
        private IUsersApi _usersApi;
        private readonly IMapper<UserResponse, UserRepresentation> _userMapper;

        public UserServiceKeycloak(IServiceConfiguration configuration, ILogger<UserServiceStub> logger, IUsersApi usersApi, IMapper<UserResponse,UserRepresentation> userMapper)
        {
            _realm = configuration.GetConfigurationValue(ConfigurationVariables.RealmToManage);
            _logger = logger;
            _usersApi = usersApi;
            _userMapper = userMapper;
        }

        public async Task CreateUser(string tenantId, UserResponse userToCreate)
        {
            await _usersApi.PostUsersAsync(_realm, _userMapper.MapTo(userToCreate));
        }

        public async Task<IList<UserResponse>> GetUsersForTenant(string tenantId)
        {
            var keycloakUsers = await _usersApi.GetUsersAsync(_realm);

            var toReturn = keycloakUsers.Select(_userMapper.MapFrom);

            return toReturn.ToList<UserResponse>();
        }
    }
}
