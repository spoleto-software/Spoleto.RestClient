namespace Spoleto.RestClient
{
    public interface IRestClient : IDisposable
    {
        JsonRestRequest<TObj> CreateJsonRestRequest<TObj>(string uri, HttpMethod httpMethod = HttpMethod.Get, bool isMultipartFormData = false, TObj? content = null) where TObj : class;

        XmlRestRequest<TObj> CreateXmlRestRequest<TObj>(string uri, HttpMethod httpMethod = HttpMethod.Get, bool isMultipartFormData = false, TObj? content = null) where TObj : class;

        BinaryRestRequest CreateBinaryRestRequest(string uri, HttpMethod httpMethod = HttpMethod.Get, bool isMultipartFormData = false, byte[]? content = null);

        Task<TextRestResponse> ExecuteAsStringAsync(RestRequest request, CancellationToken cancellationToken = default);

        Task<BinaryRestResponse> ExecuteAsBytesAsync(RestRequest request, CancellationToken cancellationToken = default);

        Task<T> ExecuteAsync<T>(RestRequest request, CancellationToken cancellationToken = default) where T : class;
    }
}
