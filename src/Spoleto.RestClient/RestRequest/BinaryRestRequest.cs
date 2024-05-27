
using System.Net.Http.Headers;

namespace Spoleto.RestClient
{
    public record BinaryRestRequest : RestRequest<byte[]>, IBinaryRestRequest
    {
        public BinaryRestRequest(string uri, HttpMethod httpMethod = HttpMethod.Get, bool isMultipartFormData = false, byte[]? content = null)
            : base(uri, httpMethod, isMultipartFormData, content)
        {
        }

        protected override string GetContentType() => ContentTypes.ApplicationOctetStream;

        public override HttpContent GetHttpContent()
        {
            HttpContent content = new ByteArrayContent(Content);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse(ContentType);

            if (IsMultipartFormData)
            {
                content = new MultipartFormDataContent { { content } };
            }

            return content;
        }
    }
}
