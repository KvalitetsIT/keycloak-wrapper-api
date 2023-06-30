using FS.Keycloak.RestApiClient.Model;
using KitNugs.Controllers;

public class KeycloakUserMapper : IMapper<UserResponse, UserRepresentation>
{
    public UserResponse MapFrom(UserRepresentation keycloakUser)
    {
        return new UserResponse()
        {
            Username = keycloakUser.Username,
            Email = keycloakUser.Email,
            FirstName = keycloakUser.FirstName,
            LastName = keycloakUser.LastName,
            RequiredActions = keycloakUser.RequiredActions
        };
    }

    public UserRepresentation MapTo(UserResponse internalUser)
    {
        return new FS.Keycloak.RestApiClient.Model.UserRepresentation()
        {
            Username = internalUser.Username,
            Email = internalUser.Email,
            FirstName = internalUser.FirstName,
            LastName = internalUser.LastName,
            RequiredActions = internalUser.RequiredActions,
        };
    }
}