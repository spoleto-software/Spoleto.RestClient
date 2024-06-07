using Spoleto.Common.Helpers;

namespace Spoleto.RestClient.Serializers
{
    public class JsonSerializer : IJsonSerializer
    {
        public bool CanDeserialize(IRestResponse restResponse)
        {
            if (restResponse is not TextRestResponse textRestResponse)
                return false;

            var content = textRestResponse?.Content;
            return IsJson(content);
        }

        public bool CanDeserialize(string raw) => IsJson(raw);

        private static bool IsJson(string? content)
        {
            if (content == null)
            {
                return false;
            }

#if NET
            return content.IndexOf('{', StringComparison.Ordinal) == 0 || content.IndexOf('[', StringComparison.Ordinal) == 0;
#else
            return content.IndexOf('{') == 0 || content.IndexOf('[') == 0;
#endif
        }

        public T Deserialize<T>(IRestResponse restResponse) where T : class
        {
            if (restResponse is not TextRestResponse textRestResponse)
                return null;

            return JsonHelper.FromJson<T>(textRestResponse.Content);
        }

        public T Deserialize<T>(string raw) where T : class
        {
            return JsonHelper.FromJson<T>(raw);
        }

        public string? Serialize<T>(T? value) where T : class => SerializeToJson(value);

        private static string? SerializeToJson<T>(T? obj) where T : class 
        {
            if (obj == null)
                return null;

            var json = JsonHelper.ToJson(obj!);

            return json;
        }
    }
}
