using Spoleto.RestClient.Authentication;
using Spoleto.RestClient.Serializers;

namespace Spoleto.RestClient
{
    public class RestClient : IRestClient
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthenticator? _authenticator;
        private readonly RestClientOptions _options;
        private readonly bool _disposeHttpClient;

        public RestClient(HttpClient httpClient, IAuthenticator? authenticator = null, RestClientOptions? options = null, bool disposeHttpClient = false)
        {
            _httpClient = httpClient;
            _authenticator = authenticator;
            _options = options ?? RestClientOptions.Default;
            _disposeHttpClient = disposeHttpClient;
        }

        public virtual JsonRestRequest<TObj> CreateJsonRestRequest<TObj>(string uri, HttpMethod httpMethod = HttpMethod.Get, TObj? content = null) where TObj : class => new(uri, httpMethod, content);

        public virtual XmlRestRequest<TObj> CreateXmlRestRequest<TObj>(string uri, HttpMethod httpMethod = HttpMethod.Get, TObj? content = null) where TObj : class => new(uri, httpMethod, content);

        public virtual BinaryRestRequest CreateBinaryRestRequest(string uri, HttpMethod httpMethod = HttpMethod.Get, byte[]? content = null) => new(uri, httpMethod, content);

        public async Task<TextRestResponse> ExecuteAsStringAsync(RestRequest request, CancellationToken cancellationToken = default)
        {
            var restResponse = await InvokeAsync<TextRestResponse>(request, cancellationToken).ConfigureAwait(false);

            return restResponse;
        }

        public async Task<BinaryRestResponse> ExecuteAsBytesAsync(RestRequest request, CancellationToken cancellationToken = default)
        {
            var restResponse = await InvokeAsync<BinaryRestResponse>(request, cancellationToken).ConfigureAwait(false);

            return restResponse;
        }

        public async Task<T> ExecuteAsync<T>(RestRequest request, CancellationToken cancellationToken = default) where T : class
        {
            var restResponse = await InvokeAsync<TextRestResponse>(request, cancellationToken).ConfigureAwait(false);

            if (restResponse == null)
            {
                throw new ArgumentNullException(nameof(restResponse));
            }

            var objectResult = SerializationManager.Deserialize<T>(restResponse);

            return objectResult;
        }

        private async Task<T> InvokeAsync<T>(RestRequest request, CancellationToken cancellationToken = default) where T : IRestResponse, new()
        {
            using var requestMessage = new HttpRequestMessage(request.HttpMethod.ConvertToHttpMethod(), request.Uri);

            if (_authenticator != null)
            {
                await _authenticator.Authenticate(this, requestMessage).ConfigureAwait(false);
            }

            requestMessage.Content = request.GetHttpContent();

            using var responseMessage = await _httpClient.SendAsync(requestMessage, cancellationToken);

            var restResponse = await responseMessage.ToRestResponse<T>(cancellationToken).ConfigureAwait(false);

            if (responseMessage.IsSuccessStatusCode)
            {
                return restResponse;
            }

            if (_authenticator is IDynamicAuthenticator dynamicAuthenticator
                && await dynamicAuthenticator.IsExpired(responseMessage).ConfigureAwait(false))
            {
                dynamicAuthenticator.SetExpired();//todo: если новый токен снова неверный, то нужна ошибка

                return await InvokeAsync<T>(request, cancellationToken).ConfigureAwait(false);
            }

            if (request.ThrowIfHttpError)
            {
                var errorResult = default(string);
                if (restResponse is ITextRestResponse textRestResponse)
                    errorResult = textRestResponse.Content;

                if (!String.IsNullOrEmpty(errorResult))
                {
                    //_logger.LogError(errorResult);

                    var exception = new Exception(errorResult);
                    exception.InitializeException(responseMessage);

                    throw exception;
                }
                else
                {
                    //_logger.LogError(responseMessage.ReasonPhrase);

                    var exception = new Exception(responseMessage.ReasonPhrase);
                    exception.InitializeException(responseMessage);

                    throw exception;
                }
            }

            return default;
        }



        #region IDisposable
        bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;
                if (_disposeHttpClient)
                {
                    _httpClient.Dispose();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
