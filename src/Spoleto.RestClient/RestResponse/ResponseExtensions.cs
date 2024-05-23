namespace Spoleto.RestClient
{
    public static class ResponseExtensions
    {
        public static async Task<T> ToRestResponse<T>(this HttpResponseMessage responseMessage, CancellationToken cancellationToken = default) where T: IRestResponse, new()
        {
            if (typeof(IBinaryRestResponse).IsAssignableFrom(typeof(T)))
            {
#if NET
                var bytes = await responseMessage.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
#else
                var bytes = await responseMessage.Content.ReadAsByteArrayAsync();
#endif
                var response = new T();
                ((IBinaryRestRequest)response).Content = bytes;

                return response;
            }
            else
            {

#if NET
                var content = await responseMessage.Content.ReadAsStringAsync(cancellationToken);
#else
            var content = await responseMessage.Content.ReadAsStringAsync();
#endif

                var response = new T();
                ((ITextRestResponse)response).Content = content;

                return response;
            }
        }
    }
}
