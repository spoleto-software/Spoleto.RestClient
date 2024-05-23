
namespace Spoleto.RestClient
{
    public interface IRestRequest
    {
        HttpMethod HttpMethod { get; }

        string Uri { get; }

        HttpContent GetHttpContent();
    }
}