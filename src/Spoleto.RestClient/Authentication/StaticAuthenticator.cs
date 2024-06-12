namespace Spoleto.RestClient.Authentication
{
    public class StaticAuthenticator : AuthenticatorBase, IStaticAuthenticator
    {
        private readonly string _tokenType;
        private readonly string _token;

        public StaticAuthenticator(string tokenType, string token)
        {
            _tokenType = tokenType ?? throw new ArgumentNullException(nameof(tokenType));
            _token = token ?? throw new ArgumentNullException(nameof(token));
        }

        protected string TokenType => _tokenType;

        protected string Token => _token;

        public override Task Authenticate(IRestClient client, HttpRequestMessage request)
        {
            AddAuthorizationHeaders(request, _tokenType, _token);

            return Task.CompletedTask;
        }
    }
}
