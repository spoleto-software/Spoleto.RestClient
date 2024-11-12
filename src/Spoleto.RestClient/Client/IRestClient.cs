namespace Spoleto.RestClient
{
    public interface IRestClient : IDisposable
    {
        RestClientOptions Options { get; }

        /// <summary>
        /// The requested resource does not exists on the server.
        /// </summary>
        bool NotFound(IRestResponse restResponse);

        Task<TextRestResponse> ExecuteAsStringAsync(RestRequest request, CancellationToken cancellationToken = default);

        Task<BinaryRestResponse> ExecuteAsBytesAsync(RestRequest request, CancellationToken cancellationToken = default);

        Task<T?> ExecuteAsync<T>(RestRequest request, CancellationToken cancellationToken = default) where T : class;
    }
}
