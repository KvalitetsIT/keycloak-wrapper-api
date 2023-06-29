namespace KitNugs.Configuration
{
    public enum ConfigurationVariables
    {
        //Validate auth-header comming from requests
        IssuerCertificate,
        AllowedIssuer,
        AllowedAudience,
        TokenValidation,

        //When calling keycloak, to create or fetch users etc
        AuthServerUrl,
        ClientSecret,
        RealmToManage,
        ClientId,
        GrantType,
        TenantAttributeName

    }

    public interface IServiceConfiguration
    {
        string GetConfigurationValue(ConfigurationVariables configurationVariable);
    }
}
