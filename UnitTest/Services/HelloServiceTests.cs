using KitNugs.Configuration;
using KitNugs.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace UnitTest.Services
{
    public class HelloServiceTests
    {
        private IServiceConfiguration serviceConfiguration;
        public IUserService helloService;

        [SetUp]
        public void Setup()
        {
            ILogger<UserServiceStub> logger = Substitute.For<ILogger<UserServiceStub>>();

            serviceConfiguration = Substitute.For<IServiceConfiguration>();
            serviceConfiguration.GetConfigurationValue(ConfigurationVariables.AllowedAudience).Returns("VALUE");

            helloService = new UserServiceStub(serviceConfiguration, logger);
        }

    }
}