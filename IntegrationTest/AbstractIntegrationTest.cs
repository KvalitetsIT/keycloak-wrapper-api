using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Networks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace IntegrationTest
{
    public abstract class AbstractIntegrationTest
    {
        protected static readonly ServiceClient client;
        protected static int servicePort = 8080;
        private static string _connectionString;

        static AbstractIntegrationTest()
        {
            // Create network
            var network = new NetworkBuilder().Build();

            HttpClient? httpClient;
            if (Debugger.IsAttached)
            {
                Environment.SetEnvironmentVariable("TokenValidation", "false");
                var server = new WebApplicationFactory<Program>().Server;
                httpClient = server.CreateClient();
            }
            else
            {
                BuildAndStartService(network);
                httpClient = new HttpClient();
            }

            client = new ServiceClient(httpClient)
            {
                BaseUrl = $"http://localhost:{servicePort}"
            };
        }

        private static void BuildAndStartService(INetwork network)
        {
            var image = new ImageFromDockerfileBuilder()
              .WithDockerfileDirectory(CommonDirectoryPath.GetSolutionDirectory(), String.Empty)
              .WithDockerfile("KitNugs/Dockerfile")
              .WithName("service-qa")
              .Build();

            image.CreateAsync()
                .Wait();

            var service = new ContainerBuilder()
                .WithImage("service-qa:latest")
                .WithPortBinding(8080, true)
                .WithPortBinding(8081, true)
                .WithName("service-qa")
                .WithNetwork(network)
                .WithEnvironment("TokenValidation", "false")
                .WithEnvironment("IssuerCertificate", "VALUE")
                .WithEnvironment("AllowedIssuer", "VALUE")
                .WithEnvironment("AllowedAudience", "VALUE")
                .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r.ForPath("/healthz").ForPort(8081)))
                .Build();

            service.StartAsync()
            .Wait();

            servicePort = service.GetMappedPublicPort(8080);
        }
    }
}