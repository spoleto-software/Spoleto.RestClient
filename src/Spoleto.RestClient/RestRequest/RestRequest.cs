using System.Text;

namespace Spoleto.RestClient
{
    public record RestRequest : IRestRequest
    {
        private readonly HttpContent? _content;
        
        private readonly Dictionary<string, string> _headers = [];

        public RestRequest(RestHttpMethod httpMethod, string uri)
            : this(httpMethod, uri, null)
        {
        }

        public RestRequest(RestHttpMethod httpMethod, string uri, HttpContent? content = null)
        {
            Method = httpMethod;
            Uri = uri;
            _content = content;
        }

        public RestHttpMethod Method { get; }

        public string Uri { get; }

        public virtual Encoding Encoding { get; set; } = Encoding.UTF8;

        public bool ThrowIfHttpError { get; set; } = true;

        public Dictionary<string, string> Headers => _headers;

        public HttpContent? GetHttpContent() => _content;

        public HttpRequestMessage ToHttpRequest()
        {
            var request = new HttpRequestMessage(Method.ConvertToHttpMethod(), Uri);
            if (_content != null)
            {
                request.Content = _content;
            }

            foreach (var header in _headers)
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return request;
        }
    }
}
