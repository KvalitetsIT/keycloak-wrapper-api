using Newtonsoft.Json;
using NUnit.Framework;

namespace IntegrationTest
{
    public class Tests : AbstractIntegrationTest
    {
        [Test]
        public void UsersAllAsync_SystemHasNoUsers_ReturnsEmptyList()
        {
            var name = Guid.NewGuid().ToString();

            Assert.DoesNotThrowAsync(() => client.UsersAllAsync(null, null));

        }

        [Test]
        public void UsersAsync_WhenUserAreCreatedItCanBeFetched()
        {

            var name = Guid.NewGuid().ToString();

            UserResponse body = new UserResponse
            {
                Username = "jens"
            };

            var creationResponse = client.UsersAsync(body).Result;
            var getUsers = client.UsersAllAsync(null, null).Result;

            Assert.AreEqual(body.Username, creationResponse.Username);
            Assert.AreEqual(1, getUsers.Count());
            Assert.AreEqual(creationResponse.Username, getUsers.First().Username);

        }

        [Test]
        public void UsersAsync_UserIsCreatedWithNullAsUsername_Error()
        {

            var name = Guid.NewGuid().ToString();

            UserResponse body = new UserResponse
            {
                Username = null
            };
            var exception = Assert.ThrowsAsync<JsonSerializationException>(() => client.UsersAsync(body));
        }
    }
}