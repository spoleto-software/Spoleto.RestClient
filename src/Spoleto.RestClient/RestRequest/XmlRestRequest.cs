using Spoleto.RestClient.Serializers;

namespace Spoleto.RestClient
{
    public record XmlRestRequest<TObj> : RestRequest<string>, IXmlRestRequest where TObj : class
    {
        public XmlRestRequest(string uri, HttpMethod httpMethod = HttpMethod.Get, TObj? obj = null)
            : base(uri, httpMethod, SerializationManager.Serialize(SerializationManager.DataFomat.Xml, obj))
        {
        }

        protected override string GetContentType() => ContentTypes.ApplicationXml;

        public override HttpContent GetHttpContent()
        {
            var content = new StringContent(Content, Encoding, ContentType);

            return content;
        }
    }
}
