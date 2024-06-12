using System.Net.Http.Headers;

namespace Spoleto.RestClient.Authentication
{
    public abstract class AuthenticatorBase : IAuthenticator
    {
        public abstract Task Authenticate(IRestClient client, HttpRequestMessage request);

        protected virtual void AddAuthorizationHeaders(HttpRequestMessage request, string tokenType, string token)
        {
            if (IsStandardTokenType(tokenType))
                request.Headers.Authorization = new AuthenticationHeaderValue(tokenType, token);
            else
                request.Headers.Add(tokenType, token);
        }

        protected virtual bool IsStandardTokenType(string tokenType)
            => tokenType == "Bearer" || tokenType == "bearer";
    }
}
