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

        public static System.Net.Http.HttpMethod ConvertToHttpMethod(this RestHttpMethod httpMethod)
             => httpMethod switch
             {
                 RestHttpMethod.Get => System.Net.Http.HttpMethod.Get,
                 RestHttpMethod.Post => System.Net.Http.HttpMethod.Post,
                 RestHttpMethod.Put => System.Net.Http.HttpMethod.Put,
#if NET
                 RestHttpMethod.Patch => System.Net.Http.HttpMethod.Patch,
#else
                 RestHttpMethod.Patch => new System.Net.Http.HttpMethod("PATCH"),
#endif
                 RestHttpMethod.Delete => System.Net.Http.HttpMethod.Delete,
                 RestHttpMethod.Head => System.Net.Http.HttpMethod.Head,

                 _ => throw new ArgumentOutOfRangeException(nameof(httpMethod))
             };

    }
}
