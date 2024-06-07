using Spoleto.RestClient.Authentication;

namespace Spoleto.RestClient
{
    public class RestClientFactory
    {
        private HttpClient? _client;
        private bool _disposeHttpClient;
        private IAuthenticator? _authenticator;
        private RestClientOptions? _options;

        public RestClientFactory WithHttpClient(HttpClient client, bool disposeHttpClient = false)
        {
            _client = client;
            _disposeHttpClient = disposeHttpClient;

            return this;
        }

        public RestClientFactory WithAuthenticator(IAuthenticator authenticator)
        {
            _authenticator = authenticator;

            return this;
        }

        public RestClientFactory WithOptions(RestClientOptions options)
        {
            _options = options;

            return this;
        }

        /// <summary>
        /// Creates the RestClient instance.
        /// </summary>
        /// <returns>Instance of <see cref="RestHttpClient"/>.</returns>
        public IRestClient Build()
        {
            if (_client == null)
            {
                _client = new HttpClient();
                _disposeHttpClient = true;
            }

            return new RestHttpClient(_client, _authenticator, _options, _disposeHttpClient);
        }
    }
}
