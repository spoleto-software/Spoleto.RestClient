
namespace Spoleto.RestClient
{
    public interface IRestRequest
    {
        HttpMethod HttpMethod { get; }
        
        bool IsMultipartFormData { get; }

        string Uri { get; }

        HttpContent GetHttpContent();
    }
}