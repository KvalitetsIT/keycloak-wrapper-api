using NUnit.Framework;

namespace IntegrationTest
{
    public class Tests : AbstractIntegrationTest
    {
        [Test]
        public void TestGetHello()
        {
            using (var httpClient = GetHttpClient())
            {
                // Use the resource
                var client = new ServiceClient(httpClient)
                {
                    BaseUrl = $"http://localhost:{servicePort}"
                };

                var name = Guid.NewGuid().ToString();

                var response = client.HelloAsync(name).Result;

                Assert.That(response, Is.Not.Null);
                Assert.That(response.Name, Is.EqualTo(name));
                Assert.That(response.From_configuration, Is.EqualTo("TEST_VARIABLE"));
            }
        }
    }
}