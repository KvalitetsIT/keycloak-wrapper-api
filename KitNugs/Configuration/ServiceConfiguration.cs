﻿namespace KitNugs.Configuration
{
    public class ServiceConfiguration : IServiceConfiguration
    {
        private readonly IDictionary<string, string> _values = new Dictionary<string, string>();

        public ServiceConfiguration(IConfiguration configuration)
        {
            foreach (string name in Enum.GetNames(typeof(ConfigurationVariables)))
            {
                try
                {
                    var key = name;
                    var value = configuration.GetValue<string>(name) ?? throw new UnsetEnvironmentVariableException(name);
                    _values[key] = value;
                    Console.WriteLine(name + ": " + value);
                }
                catch (Exception e)
                {
                    //Ignore it
                }


            }
        }

        public string GetConfigurationValue(ConfigurationVariables configurationVariable)
        {
            return _values[configurationVariable.ToString()];
        }
    }
}
