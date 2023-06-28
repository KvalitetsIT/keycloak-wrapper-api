namespace KitNugs.Configuration
{
    public enum ConfigurationVariables
    {
        IssuerCertificate,
        AllowedIssuer,
        AllowedAudience,
        TokenValidation,
        AuthServerUrl,
        ClientSecret,
        RealmToManage,
        ClientId,
        GrantType,
    }

    public interface IServiceConfiguration
    {
        string GetConfigurationValue(ConfigurationVariables configurationVariable);
    }
}
