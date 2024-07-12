using System.Net.Http.Headers;
using System.Text;
using Spoleto.Common.Helpers;
using Spoleto.RestClient.Serializers;

namespace Spoleto.RestClient
{
    /// <summary>
    /// The factory to build an instance of <see cref="RestRequest"/>.
    /// </summary>
    public class RestRequestFactory
    {
        private readonly RestHttpMethod _method;
        private readonly string _requestUri;

        private string? _query;
        private HttpContent? _content;
        private bool? _throwIfHttpError;
        private readonly Dictionary<string, string> _headers = [];

        /// <summary>
        /// Constructor with parameter.
        /// </summary>
        public RestRequestFactory(RestHttpMethod method)
            : this(method, string.Empty)
        {
        }

        /// <summary>
        /// Constructor with parameters.
        /// </summary>
        public RestRequestFactory(RestHttpMethod method, string requestUri)
        {
            if (requestUri == null)
                throw new ArgumentNullException(nameof(requestUri), "Request URI is required");

            _method = method;
            _requestUri = requestUri;
        }

        public RestRequestFactory WithQueryString(string query)
        {
            _query = query;

            return this;
        }

        public RestRequestFactory WithQueryString<T>(T querySourceObj)
        {
            _query = HttpHelper.ToQueryString(querySourceObj);

            return this;
        }

        /// <summary>
        /// application/json
        /// </summary>
        public RestRequestFactory WithJsonContent(string json, Encoding? encoding = null)
        {
            encoding ??= Encoding.UTF8;

            _content = new StringContent(json, encoding, ContentTypes.ApplicationJson);

            return this;
        }

        /// <summary>
        /// application/json
        /// </summary>
        public RestRequestFactory WithJsonContent<T>(T jsonSourceObject, Encoding? encoding = null) where T : class
        {
            encoding ??= Encoding.UTF8;
            var json = jsonSourceObject == null ? String.Empty : SerializationManager.Serialize(DataFomat.Json, jsonSourceObject)!;

            _content = new StringContent(json, encoding, ContentTypes.ApplicationJson);

            return this;
        }

        /// <summary>
        /// application/xml
        /// </summary>
        public RestRequestFactory WithXmlContent(string xml, Encoding? encoding = null)
        {
            encoding ??= Encoding.UTF8;

            _content = new StringContent(xml, encoding, ContentTypes.ApplicationXml);

            return this;
        }

        /// <summary>
        /// application/xml
        /// </summary>
        public RestRequestFactory WithXmlContent<T>(T obj, Encoding? encoding = null) where T : class
        {
            encoding ??= Encoding.UTF8;
            var xml = obj == null ? String.Empty : SerializationManager.Serialize(DataFomat.Xml, obj)!;

            _content = new StringContent(xml, encoding, ContentTypes.ApplicationXml);

            return this;
        }

        /// <summary>
        /// application/x-www-form-urlencoded
        /// </summary>
        public RestRequestFactory WithFormUrlEncodedContent(Dictionary<string, string> data)
        {
            _content = new FormUrlEncodedContent(data);

            return this;
        }

        /// <summary>
        /// application/x-www-form-urlencoded
        /// </summary>
        public RestRequestFactory WithFormUrlEncodedContent<T>(T obj)
        {
            var data = HttpHelper.ToStringDictionary(obj);

            _content = new FormUrlEncodedContent(data);

            return this;
        }

        /// <summary>
        /// multipart/form-data
        /// </summary>
        public RestRequestFactory WithMultipartFormDataContent(MultipartFormDataContent multipartContent)
        {
            _content = multipartContent;

            return this;
        }

        /// <summary>
        /// multipart/form-data
        /// </summary>
        public RestRequestFactory WithMultipartFormDataContent<T>(T obj, Encoding? encoding = null)
        {
            encoding ??= Encoding.UTF8;
            var data = HttpHelper.ToStringDictionary(obj);
            var multipartContent = new MultipartFormDataContent();

            foreach (var item in data)
            {
                var content = new StringContent(item.Value, encoding);
                multipartContent.Add(content, item.Key);
            }

            _content = multipartContent;

            return this;
        }

        public RestRequestFactory WithByteArrayContent(byte[] byteArray, string contentType = ContentTypes.ApplicationJson)
        {
            _content = new ByteArrayContent(byteArray);
            _content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            return this;
        }


        public RestRequestFactory WithStreamContent(Stream stream, string contentType = ContentTypes.ApplicationOctetStream)
        {
            _content = new StreamContent(stream);
            _content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

            return this;
        }

        public RestRequestFactory WithStringContent(string contentString, Encoding encoding, string mediaType)
        {
            _content = new StringContent(contentString, encoding, mediaType);

            return this;
        }

        public RestRequestFactory WithHeader(string name, string value)
        {
            _headers[name] = value;

            return this;
        }

        public RestRequestFactory WithBearerToken(string token)
        {
            return WithHeader("Authorization", $"Bearer {token}");
        }

        public RestRequestFactory ThrowIfHttpError(bool throwIfHttpError)
        {
            _throwIfHttpError = throwIfHttpError;

            return this;
        }

        public RestRequest Build()
        {
            var uri = _requestUri;
            if (!string.IsNullOrEmpty(_query))
            {
                var separator = _requestUri.Contains('?') ? '&' : '?';

                uri = $"{_requestUri}{separator}{_query}";
            }

            var request = new RestRequest(_method, uri, _content);
            if (_throwIfHttpError != null)
                request.ThrowIfHttpError = _throwIfHttpError.Value;

            foreach (var header in _headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }

            return request;
        }
    }
}
