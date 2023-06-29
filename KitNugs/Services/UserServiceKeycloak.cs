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
        private readonly string _tokenAttributeName;
        private readonly ILogger<UserServiceKeycloak> _logger;
        private IUsersApi _usersApi;
        private readonly IMapper<UserResponse, UserRepresentation> _userMapper;

        public UserServiceKeycloak(IServiceConfiguration configuration, ILogger<UserServiceKeycloak> logger, IUsersApi usersApi, IMapper<UserResponse, UserRepresentation> userMapper)
        {
            _realm = configuration.GetConfigurationValue(ConfigurationVariables.RealmToManage);
            _tokenAttributeName = configuration.GetConfigurationValue(ConfigurationVariables.TenantAttributeName);
            _logger = logger;
            _usersApi = usersApi;
            _userMapper = userMapper;
        }

        public async Task CreateUser(string tenantId, UserResponse userToCreate)
        {
            UserRepresentation userRepresentation = _userMapper.MapTo(userToCreate);
            var attributes = new Dictionary<string, List<string>>(){
                {_tokenAttributeName, new List<string>() { tenantId }}
            };
            userRepresentation.Attributes = attributes;

            await _usersApi.PostUsersAsync(_realm, userRepresentation);
        }

        public async Task<IList<UserResponse>> GetUsersForTenant(string tenantId)
        {
            var keycloakUsers = await _usersApi.GetUsersAsync(_realm, q: _tokenAttributeName + ":" + tenantId);

            var toReturn = keycloakUsers.Select(_userMapper.MapFrom);

            return toReturn.ToList<UserResponse>();
        }
    }
}
