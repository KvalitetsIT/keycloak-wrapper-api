namespace KitNugs.Configuration
{
    public enum ConfigurationVariables
    {
        IssuerCertificate,
        AllowedIssuer,
        AllowedAudience,
        TokenValidation,
        AuthServerUrl,
        AuthPassword,
        AuthUsername,
        RealmToManage,
        ClientId,
        GrantType,
    }

    public interface IServiceConfiguration
    {
        string GetConfigurationValue(ConfigurationVariables configurationVariable);
    }
}
