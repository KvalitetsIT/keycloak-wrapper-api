using FS.Keycloak.RestApiClient.Api;
using FS.Keycloak.RestApiClient.Model;
using KitNugs.Configuration;
using KitNugs.Controllers;
using KitNugs.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NSubstitute;

namespace UnitTest.Services.Mappers
{
    public class KeycloakUserMapperTests
    {
        private IServiceConfiguration serviceConfiguration;
        public IUserService helloService;

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void MapFrom()
        {
            var mapper = new KeycloakUserMapper();

            var keycloakUser = new UserRepresentation()
            {
                Username = "1",
                FirstName = "2",
                LastName = "3",
                RequiredActions = new List<string>() { "4" }
            };

            var internalUser = mapper.MapFrom(keycloakUser);

            Assert.AreEqual(keycloakUser.Username, internalUser.Username);
            Assert.AreEqual(keycloakUser.FirstName, internalUser.FirstName);
            Assert.AreEqual(keycloakUser.LastName, internalUser.LastName);
            Assert.AreEqual(1, internalUser.RequiredActions.Count());
            Assert.AreEqual(keycloakUser.RequiredActions.First(), internalUser.RequiredActions.First());
        }

        [Test]
        public void MapTo()
        {
            var mapper = new KeycloakUserMapper();

            var internalUser = new UserResponse()
            {
                Username = "1",
                FirstName = "2",
                LastName = "3",
                RequiredActions = new List<string>() { "4" }
            };

            var keycloakUser = mapper.MapTo(internalUser);

            Assert.AreEqual(keycloakUser.Username, internalUser.Username);
            Assert.AreEqual(keycloakUser.FirstName, internalUser.FirstName);
            Assert.AreEqual(keycloakUser.LastName, internalUser.LastName);
            Assert.AreEqual(1, internalUser.RequiredActions.Count());
            Assert.AreEqual(keycloakUser.RequiredActions.First(), internalUser.RequiredActions.First());
        }

    }
}