using System.Text;

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

            if (!String.IsNullOrEmpty(responseMessage.Content?.Headers.ContentType?.CharSet))
            {
                var encoding = Encoding.GetEncoding(responseMessage.Content.Headers.ContentType.CharSet);
                if (encoding != null)
                {
                    response.Encoding = encoding;
                }
            }

            if (typeof(IBinaryRestResponse).IsAssignableFrom(typeof(T)))
            {
#if NET
                var bytes = await responseMessage.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
#else
                var bytes = await responseMessage.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
#endif
                ((IBinaryRestResponse)response).Content = bytes;

                return response;
            }
            else
            {

#if NET
                var content = await responseMessage.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
#else
                var content = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
#endif
                ((ITextRestResponse)response).Content = content;

                return response;
            }
        }
    }
}
