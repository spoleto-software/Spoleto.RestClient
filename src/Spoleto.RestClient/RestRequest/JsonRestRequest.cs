
using Spoleto.Common.Helpers;
using Spoleto.RestClient.Serializers;

namespace Spoleto.RestClient
{
    public record JsonRestRequest<TObj> : RestRequest<string>, IJsonRestRequest where TObj : class
    {
        public JsonRestRequest(string uri, HttpMethod httpMethod = HttpMethod.Get, bool isMultipartFormData = false, TObj? obj = null)
            : base(uri, httpMethod, isMultipartFormData, SerializationManager.Serialize(SerializationManager.DataFomat.Json, obj))
        {
        }

        protected override string GetContentType() => ContentTypes.ApplicationJson;

        public override HttpContent GetHttpContent()
        {
            HttpContent content = new StringContent(Content, Encoding, ContentType);

            if (IsMultipartFormData)
            {
                content = new MultipartFormDataContent { { content } };
            }

            return content;
        }

        private static string? SerializeToJson(TObj? obj)
        {
            if (obj == null)
                return null;

            var json = JsonHelper.ToJson(obj!);

            return json;
        }
    }
}
