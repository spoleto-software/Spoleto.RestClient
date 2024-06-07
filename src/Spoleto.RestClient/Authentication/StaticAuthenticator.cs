using System.Net.Http.Headers;

namespace Spoleto.RestClient.Authentication
{
    public sealed class StaticAuthenticator : AuthenticatorBase, IStaticAuthenticator
    {
        private readonly string _tokenType;
        private readonly string _token;

        public StaticAuthenticator(string tokenType, string token)
        {
            _tokenType = tokenType ?? throw new ArgumentNullException(nameof(tokenType));
            _token = token ?? throw new ArgumentNullException(nameof(token));
        }

        public override Task Authenticate(IRestClient client, HttpRequestMessage request)
        {
            if (IsStandardTokenType())
                request.Headers.Authorization = new AuthenticationHeaderValue(_tokenType, _token);
            else
                request.Headers.Add(_tokenType, _token);

            return Task.CompletedTask;
        }

        private bool IsStandardTokenType()
        {
            return _tokenType == "Bearer" || _tokenType == "bearer";
               
        }
    }
}
