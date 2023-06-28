using System;

namespace FS.Keycloak.RestApiClient.Client
{
    public static class CustomApiClientFactory
    {
        public static TApiClient Create<TApiClient>(CustomKeycloakHttpClient httpClient) where TApiClient : IApiAccessor
            => (TApiClient)Activator
                .CreateInstance(
                    typeof(TApiClient),
                    httpClient,
                    new Configuration { BasePath = $"{httpClient.AuthServerUrl}/admin/realms" },
                    null
                );
    }
}