namespace Spoleto.RestClient.Authentication
{
    public interface IAuthenticator
    {
        Task Authenticate(IRestClient client, HttpRequestMessage request);
    }
}
