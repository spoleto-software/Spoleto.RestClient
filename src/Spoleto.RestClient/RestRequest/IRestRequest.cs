
using System.Text;

namespace Spoleto.RestClient
{
    public interface IRestRequest
    {
        RestHttpMethod Method { get; }
        
        string Uri { get; }

        Encoding Encoding { get; set; }

        bool ThrowIfHttpError { get; set; }

        Dictionary<string, string> Headers { get; }

        HttpContent? GetHttpContent();
    }
}