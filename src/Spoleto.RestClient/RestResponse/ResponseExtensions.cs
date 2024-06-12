namespace Spoleto.RestClient
{
    public static class ResponseExtensions
    {
        public static async Task<T> ToRestResponse<T>(this HttpResponseMessage responseMessage, CancellationToken cancellationToken = default) where T : IRestResponse, new()
        {
            var response = new T
            {
                IsSuccessStatusCode = responseMessage.IsSuccessStatusCode,
                StatusCode = responseMessage.StatusCode,
                ContentType = responseMessage.Content?.Headers.ContentType?.MediaType
            };

            if (typeof(IBinaryRestResponse).IsAssignableFrom(typeof(T)))
            {
#if NET
                var bytes = await responseMessage.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
#else
                var bytes = await responseMessage.Content.ReadAsByteArrayAsync();
#endif
                ((IBinaryRestResponse)response).Content = bytes;

                return response;
            }
            else
            {

#if NET
                var content = await responseMessage.Content.ReadAsStringAsync(cancellationToken);
#else
                var content = await responseMessage.Content.ReadAsStringAsync();
#endif
                ((ITextRestResponse)response).Content = content;

                return response;
            }
        }
    }
}
