using System.Net.Http.Headers;

namespace Spoleto.RestClient.Authentication
{
    public sealed class StaticAuthenticator : AuthenticatorBase, IStaticAuthenticator
    {
        private readonly string _token;
        private readonly string _tokenType;

        public StaticAuthenticator(string token, string tokenType)
        {
            _token = token ?? throw new ArgumentNullException(nameof(token));
            _tokenType = tokenType ?? throw new ArgumentNullException(nameof(tokenType));
        }

        public override Task Authenticate(IRestClient client, HttpRequestMessage request)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue(_tokenType, _token);

            return Task.CompletedTask;
        }
    }
}
