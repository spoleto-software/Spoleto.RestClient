
using System.Net.Http.Headers;

namespace Spoleto.RestClient
{
    public record BinaryRestRequest : RestRequest<byte[]>, IBinaryRestRequest
    {
        public BinaryRestRequest(string uri, HttpMethod httpMethod = HttpMethod.Get, byte[]? content = null)
            : base(uri, httpMethod, content)
        {
        }

        protected override string GetContentType() => ContentTypes.ApplicationOctetStream;

        public override HttpContent GetHttpContent()
        {
            var content = new ByteArrayContent(Content);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse(ContentType);

            return content;
        }
    }
}
