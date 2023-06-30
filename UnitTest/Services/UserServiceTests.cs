using FS.Keycloak.RestApiClient.Api;
using FS.Keycloak.RestApiClient.Model;
using KitNugs.Configuration;
using KitNugs.Controllers;
using KitNugs.Services;
using Microsoft.Extensions.Logging;
using Moq;
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

        }

        [Test]
        public void GetUsers_CalledWithTenantId_AndMapperIsCalled()
        {
            var configuration = new Mock<IServiceConfiguration>();
            var logger = new Mock<ILogger<UserServiceKeycloak>>();
            var userapi = new Mock<IUsersApi>();
            var mapper = new Mock<IMapper<UserResponse, UserRepresentation>>();

            var tenantId = "1";
            var realm = "realm1";
            var tenantAttributeName = "tenantId";

            configuration.Setup(p => p.GetConfigurationValue(ConfigurationVariables.RealmToManage)).Returns(realm);
            configuration.Setup(p => p.GetConfigurationValue(ConfigurationVariables.TenantAttributeName)).Returns(tenantAttributeName);

            var userService = new UserServiceKeycloak(configuration.Object, logger.Object, userapi.Object, mapper.Object);
            var userToCreate = new UserResponse()
            {
                FirstName = "Jens",
                LastName = "Jens",
                Username = "jensen"
            };
            var keycloakUser = new UserRepresentation()
            {
                FirstName = "Jens",
                LastName = "Jens",
                Username = "jensen"
            };

            userapi.Setup(p => p.GetUsersAsync(
                realm,
                null, null, null, null, null, null, null, null, 0, 5, null, null, null,
                tenantAttributeName + ":" + tenantId,
                It.IsAny<CancellationToken>()
            )).Returns(Task.FromResult(new List<UserRepresentation>() { keycloakUser }));

            mapper.Setup(p => p.MapFrom(keycloakUser)).Returns(userToCreate);

            var allUsersForTenant = userService.GetUsersForTenant(tenantId, 1, 5).Result;

            mapper.Verify(mapper => mapper.MapFrom(keycloakUser), Times.Once());
            userapi.Verify(userapi => userapi.GetUsersAsync(
                realm,
                null, null, null, null, null, null, null, null, 0, 5, null, null, null,
                tenantAttributeName + ":" + tenantId,
                It.IsAny<CancellationToken>()
            ), Times.Once());

        }

        [Test]
        public void CreateUser_AttributesAreMappedCorrectly()
        {
            var configuration = new Mock<IServiceConfiguration>();
            var logger = new Mock<ILogger<UserServiceKeycloak>>();
            var userapi = new Mock<IUsersApi>();
            var mapper = new Mock<IMapper<UserResponse, UserRepresentation>>();

            var tenantId = "1";
            var realm = "realm1";
            var tenantAttributeName = "tenantId";

            configuration.Setup(p => p.GetConfigurationValue(ConfigurationVariables.RealmToManage)).Returns(realm);
            configuration.Setup(p => p.GetConfigurationValue(ConfigurationVariables.TenantAttributeName)).Returns(tenantAttributeName);


            var userService = new UserServiceKeycloak(configuration.Object, logger.Object, userapi.Object, mapper.Object);
            var userToCreate = new UserResponse()
            {
                FirstName = "Jens",
                LastName = "Jens",
                Username = "jensen"
            };
            var keycloakUser = new UserRepresentation()
            {
                FirstName = "Jens",
                LastName = "Jens",
                Username = "jensen"
            };

            mapper.Setup(p => p.MapTo(userToCreate)).Returns(keycloakUser);
            userService.CreateUser(tenantId, userToCreate).Wait();

            mapper.Verify(mapper => mapper.MapTo(userToCreate), Times.Once());
            userapi.Verify(userapi => userapi.PostUsersAsync(realm, It.Is<UserRepresentation>(user =>
                user.FirstName.Equals(keycloakUser.FirstName) &&
                user.LastName.Equals(keycloakUser.LastName) &&
                user.Username.Equals(keycloakUser.Username) &&
                user.Attributes.Count() == 1 &&
                user.Attributes.First().Key.Equals(tenantAttributeName) &&
                user.Attributes.First().Value.Count() == 1 &&
                user.Attributes.First().Value.First().Equals(tenantId)
            ), It.IsAny<CancellationToken>()), Times.Once());

        }

    }
}