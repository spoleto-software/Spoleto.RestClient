namespace Spoleto.RestClient.Authentication
{
    public abstract class AuthenticatorBase : IAuthenticator
    {
        public abstract Task Authenticate(IRestClient client, HttpRequestMessage request);
    }
}
