﻿using System.Net;
using System.Security.Authentication;

namespace Spoleto.RestClient.Authentication
{
    public abstract class DynamicAuthenticator : AuthenticatorBase, IDynamicAuthenticator
    {
        private readonly SemaphoreSlim _semaphore = new(1, 1);
        private readonly string _tokenType;

        private string? _token;
        private bool _inProcess = false;


        public DynamicAuthenticator(string tokenType)
        {
            _tokenType = tokenType ?? throw new ArgumentNullException(nameof(tokenType));
        }

        protected string TokenType => _tokenType;

        protected string? Token => _token;

        public sealed override async Task Authenticate(IRestClient client, HttpRequestMessage request)
        {
            if (_token == null)
            {
                if (!_inProcess)
                {
                    await _semaphore.WaitAsync().ConfigureAwait(false);
                    try
                    {
                        if (_token == null)
                        {
                            _inProcess = true;

                            // Get a new token:
                            _token = await GetAuthenticationToken(client).ConfigureAwait(false);

                            _inProcess = false;
                        }
                    }
                    finally
                    {
                        _semaphore.Release();
                    }
                }
            }

            if (_token != null)
            {
                AddAuthorizationHeaders(request, _tokenType, _token);
            }
        }

        public virtual Task<bool> IsExpired(HttpResponseMessage response) => Task.FromResult(response.StatusCode == HttpStatusCode.Unauthorized);

        public virtual void SetExpired()
        {
            if (_token == null)
                throw new AuthenticationException("Cannot authenticate and get an access token. Check the credentials.");

            _token = null;
        }

        protected abstract Task<string> GetAuthenticationToken(IRestClient client);

        #region IDisposable
        bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;
                _token = null;
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
