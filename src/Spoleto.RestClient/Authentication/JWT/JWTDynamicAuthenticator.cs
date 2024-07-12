namespace Spoleto.RestClient.Authentication.JWT
{
    public abstract class JWTDynamicAuthenticator<TCredentials, TToken> : DynamicAuthenticator
        where TCredentials : class
        where TToken : class, IJwtToken
    {
        private const string _tokenType = "Bearer";

        private readonly TCredentials _credentials;
        private TToken? _tokenModel;

        public JWTDynamicAuthenticator(TCredentials credentials)
            : base(_tokenType)
        {
            if (credentials is null)
            {
                throw new ArgumentNullException(nameof(credentials));
            }

            _credentials = credentials;
        }

        protected override async Task<string> GetAuthenticationToken(IRestClient client)
        {
            if (_tokenModel == null) // get a new token
            {
                _tokenModel = await GetAccessToken(client, _credentials).ConfigureAwait(false);
            }
            else // refresh the token
            {
                try
                {
                    _tokenModel = await RefreshAccessToken(client, _credentials, _tokenModel).ConfigureAwait(false);
                }
                catch
                {
                    _tokenModel = null;

                    return await GetAuthenticationToken(client).ConfigureAwait(false);
                }
            }

            return _tokenModel.AccessToken;
        }

        protected abstract Task<TToken> GetAccessToken(IRestClient client, TCredentials credentials);

        protected abstract Task<TToken> RefreshAccessToken(IRestClient client, TCredentials credentials, TToken token);
    }
}
