using FS.Keycloak.RestApiClient.Model;
using KitNugs.Controllers;

public class KeycloakUserMapper : IMapper<UserResponse, UserRepresentation>
{
    public UserResponse MapFrom(UserRepresentation keycloakUser)
    {
        return new UserResponse()
        {
            Username = keycloakUser.Username
        };
    }

    public UserRepresentation MapTo(UserResponse internalUser)
    {
        return new FS.Keycloak.RestApiClient.Model.UserRepresentation()
        {
            Username = internalUser.Username
        };
    }
}