namespace KitNugs.Configuration
{
    public enum ConfigurationVariables
    {
        IssuerCertificate,
        AllowedIssuer,
        AllowedAudience,
        TokenValidation
    }

    public interface IServiceConfiguration
    {
        string GetConfigurationValue(ConfigurationVariables configurationVariable);
    }
}
