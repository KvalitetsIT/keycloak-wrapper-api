using FS.Keycloak.RestApiClient.Api;
using FS.Keycloak.RestApiClient.Client;
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
        public UserServiceKeycloak(IServiceConfiguration configuration, ILogger<UserServiceStub> logger, IUsersApi usersApi)
        {
            _realm = configuration.GetConfigurationValue(ConfigurationVariables.RealmToManage);
            _logger = logger;
            _usersApi = usersApi;
        }

        public async Task CreateUser(UserResponse userToCreate)
        {
            await _usersApi.PostUsersAsync(_realm, mapFromInternalToKeycloakUser(userToCreate));
        }

        public async Task<IList<UserResponse>> GetUsers()
        {
            var keycloakUsers = await _usersApi.GetUsersAsync(_realm);

            var toReturn = keycloakUsers.Select(mapFromKeycloakUserToInternal);

            return toReturn.ToList<UserResponse>();
        }

        private UserResponse mapFromKeycloakUserToInternal(FS.Keycloak.RestApiClient.Model.UserRepresentation keycloakUser)
        {
            return new UserResponse()
            {
                Username = keycloakUser.Username
            };
        }

        private FS.Keycloak.RestApiClient.Model.UserRepresentation mapFromInternalToKeycloakUser(UserResponse internalUser)
        {
            return new FS.Keycloak.RestApiClient.Model.UserRepresentation()
            {
                Username = internalUser.Username
            };
        }
    }
}
