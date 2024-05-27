using Spoleto.RestClient.Serializers;

namespace Spoleto.RestClient
{
    public record XmlRestRequest<TObj> : RestRequest<string>, IXmlRestRequest where TObj : class
    {
        public XmlRestRequest(string uri, HttpMethod httpMethod = HttpMethod.Get, bool isMultipartFormData = false, TObj? obj = null)
            : base(uri, httpMethod, isMultipartFormData, SerializationManager.Serialize(SerializationManager.DataFomat.Xml, obj))
        {
        }

        protected override string GetContentType() => ContentTypes.ApplicationXml;

        public override HttpContent GetHttpContent()
        {
            HttpContent content = new StringContent(Content, Encoding, ContentType);

            if (IsMultipartFormData)
            {
                content = new MultipartFormDataContent { { content } };
            }

            return content;
        }
    }
}
