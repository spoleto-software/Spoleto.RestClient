using System.Text;

namespace Spoleto.RestClient
{
    public abstract record RestRequest : IRestRequest
    {
        protected RestRequest(string uri, HttpMethod httpMethod = HttpMethod.Get, bool isMultipartFormData = false)
        {
            Uri = uri;
            HttpMethod = httpMethod;
            IsMultipartFormData = isMultipartFormData;
            ContentType = GetContentType();
        }

        public string Uri { get; }

        public HttpMethod HttpMethod { get; }

        public bool IsMultipartFormData { get; }

        public string ContentType { get; set; }

        public virtual Encoding Encoding { get; set; } = Encoding.UTF8;

        public bool ThrowIfHttpError { get; set; } = true;

        protected abstract string GetContentType();

        public abstract HttpContent GetHttpContent();
    }

    public abstract record RestRequest<T> : RestRequest, IRestRequestGeneric<T> where T : class
    {
        protected RestRequest(string uri, HttpMethod httpMethod = HttpMethod.Get,bool isMultipartFormData=false, T? content = null)
            : base(uri, httpMethod)
        {
            Content = content;
        }

        public T? Content { get; set; }
    }
}
