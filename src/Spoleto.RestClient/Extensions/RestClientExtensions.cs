using Spoleto.RestClient.Serializers;

namespace Spoleto.RestClient
{
    public static class RestClientExtensions
    {
        public static async Task<(T? Object, string RawBody)> ExecuteWithRawBodyAsync<T>(this IRestClient client, RestRequest request, CancellationToken cancellationToken = default) where T : class
        {
            var restResponse = await client.ExecuteAsStringAsync(request, cancellationToken).ConfigureAwait(false);

            if (restResponse == null)
            {
                throw new ArgumentNullException(nameof(restResponse));
            }

            if (client.NotFound(restResponse)
                && !client.Options.ThrowExceptionIfNotFound)
            {
                return (default, restResponse.Content);
            }

            if (!restResponse.IsSuccessStatusCode)
            {
                throw new Exception($"Unsuccesful response with {nameof(restResponse.StatusCode)} = {restResponse.StatusCode}");
            }

            if (string.IsNullOrEmpty(restResponse.Content))
            {
                return (default, restResponse.Content);
            }

            var objectResult = SerializationManager.Deserialize<T>(restResponse);

            return (objectResult, restResponse.Content);
        }
    }
}
