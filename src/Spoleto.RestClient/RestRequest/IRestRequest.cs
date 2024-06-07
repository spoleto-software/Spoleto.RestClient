
namespace Spoleto.RestClient
{
    public interface IRestRequest
    {
        RestHttpMethod Method { get; }
        
        string Uri { get; }

        HttpContent? GetHttpContent();
    }
}