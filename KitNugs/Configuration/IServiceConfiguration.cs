namespace KitNugs.Configuration
{
    public enum ConfigurationVariables
    {
        IssuerCertificate,
        AllowedIssuer,
        AllowedAudience
    }

    public interface IServiceConfiguration
    {
        string GetConfigurationValue(ConfigurationVariables configurationVariable);
    }
}
