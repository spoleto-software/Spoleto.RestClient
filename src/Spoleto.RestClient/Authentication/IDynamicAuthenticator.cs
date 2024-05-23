namespace Spoleto.RestClient.Authentication
{
    public interface IDynamicAuthenticator : IAuthenticator, IDisposable
    {
        Task<bool> IsExpired(HttpResponseMessage response);

        void SetExpired();
    }
}
