﻿using Spoleto.RestClient.Authentication;
using Spoleto.RestClient.Serializers;

namespace Spoleto.RestClient
{
    public class RestHttpClient : IRestClient
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthenticator? _authenticator;
        private readonly RestClientOptions _options;//todo:
        private readonly bool _disposeHttpClient;

        public RestHttpClient(IAuthenticator? authenticator = null, RestClientOptions? options = null)
            : this(new HttpClient(), authenticator, options, true)
        {
        }

        public RestHttpClient(HttpClient httpClient, IAuthenticator? authenticator = null, RestClientOptions? options = null, bool disposeHttpClient = false)
        {
            _httpClient = httpClient;
            _authenticator = authenticator;
            _options = options ?? RestClientOptions.Default;
            _disposeHttpClient = disposeHttpClient;
        }

        public RestClientOptions Options => _options;

        /// <summary>
        /// The requested resource does not exists on the server.
        /// </summary>
        public virtual bool NotFound(IRestResponse restResponse)
            => restResponse.StatusCode == System.Net.HttpStatusCode.NotFound;

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

        public async Task<T?> ExecuteAsync<T>(RestRequest request, CancellationToken cancellationToken = default) where T : class
        {
            var restResponse = await InvokeAsync<TextRestResponse>(request, cancellationToken).ConfigureAwait(false);

            if (restResponse == null)
            {
                throw new ArgumentNullException(nameof(restResponse));
            }

            if (NotFound(restResponse)
                && !_options.ThrowExceptionIfNotFound)
            {
                return default;
            }

            if (!restResponse.IsSuccessStatusCode)
            {
                throw new Exception($"Unsuccesful response with {nameof(restResponse.StatusCode)} = {restResponse.StatusCode}");
            }


            if (string.IsNullOrEmpty(restResponse.Content))
            {
                return default;
            }

            var objectResult = SerializationManager.Deserialize<T>(restResponse);

            return objectResult;
        }

        private async Task<T> InvokeAsync<T>(RestRequest request, CancellationToken cancellationToken = default) where T : IRestResponse, new()
        {
            using var requestMessage = request.ToHttpRequest();

            if (_authenticator != null)
            {
                await _authenticator.Authenticate(this, requestMessage).ConfigureAwait(false);
            }

            using var responseMessage = await _httpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);

            if (_authenticator is IDynamicAuthenticator dynamicAuthenticator
                && await dynamicAuthenticator.IsExpired(responseMessage).ConfigureAwait(false))
            {
                dynamicAuthenticator.SetExpired();//todo: если новый токен снова неверный, то нужна ошибка

                var result = await InvokeAsync<T>(request, cancellationToken).ConfigureAwait(false);

                return result;
            }

            var restResponse = await responseMessage.ToRestResponse<T>(cancellationToken).ConfigureAwait(false);

            if (restResponse.IsSuccessStatusCode)
            {
                return restResponse;
            }

            if (NotFound(restResponse)
                && !_options.ThrowExceptionIfNotFound)
            {
                return restResponse;
            }

            if (request.ThrowIfHttpError)
            {
                var errorResult = default(string);
                if (restResponse is ITextRestResponse textRestResponse)
                {
                    errorResult = textRestResponse.Content;
                }
                else if (restResponse is IBinaryRestResponse binaryRestResponse
                     && binaryRestResponse.Content != null)
                {
                    errorResult = restResponse.Encoding.GetString(binaryRestResponse.Content);
                }

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

            return restResponse;
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
