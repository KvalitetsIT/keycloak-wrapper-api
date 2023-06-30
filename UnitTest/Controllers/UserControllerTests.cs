using System.IdentityModel.Tokens.Jwt;
using FS.Keycloak.RestApiClient.Api;
using FS.Keycloak.RestApiClient.Model;
using KitNugs.Configuration;
using KitNugs.Controllers;
using KitNugs.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NSubstitute;

namespace UnitTest.Controllers
{
    public class UserControllerTests
    {
        private IServiceConfiguration serviceConfiguration;
        public IUserService helloService;

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void PostUsers_UserServiceIsCalledCorrectly()
        {
            var configuration = new Mock<IServiceConfiguration>();
            var logger = new Mock<ILogger<UserController>>();
            var userapi = new Mock<IUsersApi>();
            var mapper = new Mock<IMapper<UserResponse, UserRepresentation>>();
            var userservice = new Mock<IUserService>();
            var exceptionHandler = new Mock<IExceptionHandler>();
            var tokenHandler = new Mock<ITokenHandler>();

            var tenantId = "tenant1";
            tokenHandler.Setup(tokenhandler => tokenhandler.ExtractTenantIdFromToken(It.IsAny<JwtSecurityToken>())).Returns(tenantId);

            var controller = new UserController(logger.Object, configuration.Object, userservice.Object, exceptionHandler.Object, tokenHandler.Object);
            var userToCreate = new UserResponse()
            {
                FirstName = "Jens",
                LastName = "Jensen",
                Username = "Jens@jensen.dk",
                RequiredActions = new List<string> { "action" }
            };
            var result = controller.Users(userToCreate).Result;
            userservice.Verify(userservice => userservice.CreateUser(tenantId, userToCreate), Times.Once());
        }
        [Test]
        public void PostUsers_UserServiceThrowsError_ExceptionHandlerIsCalled()
        {
            var configuration = new Mock<IServiceConfiguration>();
            var logger = new Mock<ILogger<UserController>>();
            var userapi = new Mock<IUsersApi>();
            var mapper = new Mock<IMapper<UserResponse, UserRepresentation>>();
            var userservice = new Mock<IUserService>();
            var exceptionHandler = new Mock<IExceptionHandler>();
            var tokenHandler = new Mock<ITokenHandler>();

            var tenantId = "tenant1";
            tokenHandler.Setup(tokenhandler => tokenhandler.ExtractTenantIdFromToken(It.IsAny<JwtSecurityToken>())).Returns(tenantId);
            userservice.Setup(userservice => userservice.CreateUser(It.IsAny<string>(), It.IsAny<UserResponse>())).Throws(new Exception("test exception"));

            var controller = new UserController(logger.Object, configuration.Object, userservice.Object, exceptionHandler.Object, tokenHandler.Object);
            var userToCreate = new UserResponse()
            {
                FirstName = "Jens",
                LastName = "Jensen",
                Username = "Jens@jensen.dk",
                RequiredActions = new List<string> { "action" }
            };
            var result = controller.Users(userToCreate).Result;
            exceptionHandler.Verify(userservice => userservice.HandleException(It.IsAny<Exception>()), Times.Once());
        }

        [Test]
        public void GetUsersAll_UserServiceIsCalledCorrectly()
        {
            var configuration = new Mock<IServiceConfiguration>();
            var logger = new Mock<ILogger<UserController>>();
            var userapi = new Mock<IUsersApi>();
            var mapper = new Mock<IMapper<UserResponse, UserRepresentation>>();
            var userservice = new Mock<IUserService>();
            var exceptionHandler = new Mock<IExceptionHandler>();
            var tokenHandler = new Mock<ITokenHandler>();

            var tenantId = "tenant1";
            tokenHandler.Setup(tokenhandler => tokenhandler.ExtractTenantIdFromToken(It.IsAny<JwtSecurityToken>())).Returns(tenantId);

            var controller = new UserController(logger.Object, configuration.Object, userservice.Object, exceptionHandler.Object, tokenHandler.Object);
            var result = controller.UsersAll(null, null);
            userservice.Verify(userservice => userservice.GetUsersForTenant(tenantId,null,null), Times.Once());
        }

        [Test]
        public void GetUsersAll_UserServiceThrowsError_ExceptionHandlerIsCalled()
        {
            var configuration = new Mock<IServiceConfiguration>();
            var logger = new Mock<ILogger<UserController>>();
            var userapi = new Mock<IUsersApi>();
            var mapper = new Mock<IMapper<UserResponse, UserRepresentation>>();
            var userservice = new Mock<IUserService>();
            var exceptionHandler = new Mock<IExceptionHandler>();
            var tokenHandler = new Mock<ITokenHandler>();

            var tenantId = "tenant1";
            tokenHandler.Setup(tokenhandler => tokenhandler.ExtractTenantIdFromToken(It.IsAny<JwtSecurityToken>())).Returns(tenantId);
            userservice.Setup(userservice => userservice.GetUsersForTenant(It.IsAny<string>(),null,null)).Throws(new Exception("test exception"));

            var controller = new UserController(logger.Object, configuration.Object, userservice.Object, exceptionHandler.Object, tokenHandler.Object);
            var result = controller.UsersAll(null, null);
            exceptionHandler.Verify(userservice => userservice.HandleException(It.IsAny<Exception>()), Times.Once());
        }
    }
}