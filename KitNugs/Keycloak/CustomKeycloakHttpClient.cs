using FS.Keycloak.RestApiClient.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace FS.Keycloak.RestApiClient.Client
{
    public class CustomKeycloakHttpClient : HttpClient
    {
        private readonly string _authTokenUrl;
        private readonly string _user;
        private readonly string _password;
        private readonly string _clientId;
        private readonly string _grantType;
        private KeycloakApiToken _token;
         private static readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings { ContractResolver = new CustomSnakeCaseContractResolver() };

        public string AuthServerUrl { get; }

        public CustomKeycloakHttpClient(string authServerUrl, string realm, string clientId, string grantType, string user, string password)
            : this(authServerUrl, realm, clientId, grantType, user, password, new HttpClientHandler(), true) { }

        public CustomKeycloakHttpClient(string authServerUrl, string realm, string clientId, string grantType, string user, string password, HttpMessageHandler handler, bool disposeHandler)
            : base(handler, disposeHandler)
        {
            _authTokenUrl = $"{authServerUrl}/realms/{realm}/protocol/openid-connect/token";
            _user = user;
            _password = password;
            _clientId = clientId;
            _grantType = grantType;

            AuthServerUrl = authServerUrl;
        }

        public override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await AddAuthorizationHeader(request, cancellationToken);
            return await base.SendAsync(request, cancellationToken);
        }

        private async Task AddAuthorizationHeader(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_token == null || _token.IsExpired)
                _token = await GetToken(cancellationToken);
            request.Headers.Authorization = new AuthenticationHeaderValue("bearer", _token.AccessToken);
        }

        private async Task<KeycloakApiToken> GetToken(CancellationToken cancellationToken)
        {
            var parameters = new Dictionary<string, string>
            {
                { "client_id", _clientId },
                { "client_secret", _password },
                { "grant_type", _grantType }
            };

            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, _authTokenUrl) { Content = new FormUrlEncodedContent(parameters) };
            var response = await base.SendAsync(tokenRequest, cancellationToken);
            var tokenJson = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<KeycloakApiToken>(tokenJson,_jsonSerializerSettings);
            return token;
        }
    }
}