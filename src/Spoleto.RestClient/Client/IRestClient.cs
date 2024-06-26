﻿namespace Spoleto.RestClient
{
    public interface IRestClient : IDisposable
    {
        Task<TextRestResponse> ExecuteAsStringAsync(RestRequest request, CancellationToken cancellationToken = default);

        Task<BinaryRestResponse> ExecuteAsBytesAsync(RestRequest request, CancellationToken cancellationToken = default);

        Task<T> ExecuteAsync<T>(RestRequest request, CancellationToken cancellationToken = default) where T : class;
    }
}
