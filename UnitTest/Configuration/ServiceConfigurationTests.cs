using KitNugs.Configuration;
using Microsoft.Extensions.Configuration;

namespace UnitTest.Configuration
{
    internal class ServiceConfigurationTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestBusinessLogic()
        {
            var inMemorySettings = new Dictionary<string, string> {
                {"TokenValidation", "false"},
                {"IssuerCertificate", "false"},
                {"AllowedIssuer", "false"},
                {"AllowedAudience", "false"},
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var serviceConfiguration = new ServiceConfiguration(configuration);

            var result = serviceConfiguration.GetConfigurationValue(ConfigurationVariables.AllowedAudience);

            Assert.That(result, Is.EqualTo("false"));
        }
    }
}
