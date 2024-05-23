namespace Spoleto.RestClient
{
    internal static class HttpExtensions
    {
        public static void InitializeException(this Exception exception, HttpResponseMessage responseMessage)
        {
            exception.Data.Add(nameof(responseMessage.StatusCode), responseMessage.StatusCode);
            exception.Data.Add(nameof(responseMessage.ReasonPhrase), responseMessage.ReasonPhrase);

            if (responseMessage.RequestMessage?.RequestUri != null)
                exception.Data.Add(nameof(responseMessage.RequestMessage.RequestUri), responseMessage.RequestMessage?.RequestUri);

        }

        public static System.Net.Http.HttpMethod ConvertToHttpMethod(this HttpMethod httpMethod)
             => httpMethod switch
             {
                 HttpMethod.Get => System.Net.Http.HttpMethod.Get,
                 HttpMethod.Post => System.Net.Http.HttpMethod.Post,
                 HttpMethod.Put => System.Net.Http.HttpMethod.Put,
#if NET
                 HttpMethod.Patch => System.Net.Http.HttpMethod.Patch,
#else
                 HttpMethod.Patch => new System.Net.Http.HttpMethod("PATCH"),
#endif
                 HttpMethod.Delete => System.Net.Http.HttpMethod.Delete,
                 HttpMethod.Head => System.Net.Http.HttpMethod.Head,

                 _ => throw new ArgumentOutOfRangeException(nameof(httpMethod))
             };

    }
}
