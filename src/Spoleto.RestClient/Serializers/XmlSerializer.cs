﻿namespace Spoleto.RestClient.Serializers
{
    public class XmlSerializer : IXmlSerializer
    {
        public bool CanDeserialize(IRestResponse restResponse)
        {
            if (restResponse is not TextRestResponse textRestResponse)
                return false;

            var content = textRestResponse?.Content;

            return IsXml(content);
        }

        public bool CanDeserialize(string raw) => IsXml(raw);

        private static bool IsXml(string? content)
        {
            if (content == null)
            {
                return false;
            }

#if NET
            return content.IndexOf('<', StringComparison.Ordinal) == 0;
#else
            return content.IndexOf('<') == 0;
#endif
        }

        public T Deserialize<T>(IRestResponse restResponse) where T : class
        {
            if (restResponse is not TextRestResponse textRestResponse)
                return null;

            return DeserializeFromXml<T>(textRestResponse.Content);
        }

        public T Deserialize<T>(string raw) where T : class
        {
            return DeserializeFromXml<T>(raw);
        }

        public string? Serialize<T>(T? value) where T : class => SerializeToXml(value);

        private static T? DeserializeFromXml<T>(string xml) where T : class
        {
            if (xml == null)
                return null;

            var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using StringReader stringReader = new StringReader(xml);
            return (T?)xmlSerializer.Deserialize(stringReader);
        }

        private static string? SerializeToXml<T>(T? obj)
        {
            if (obj == null)
                return null;

            //todo: use XDocument
            var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using var stringWriter = new StringWriter();
            xmlSerializer.Serialize(stringWriter, obj);

            return stringWriter.ToString();
        }
    }
}
