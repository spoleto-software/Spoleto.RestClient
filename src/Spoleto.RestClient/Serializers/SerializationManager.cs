namespace Spoleto.RestClient.Serializers
{
    public static class SerializationManager
    {
        static SerializationManager()
        {
            Serializers =
            [
                new JsonSerializer(),
                new XmlSerializer()
            ];
        }

        public static SerializerCollection Serializers { get; }

        public static T Deserialize<T>(IRestResponse restResponse) where T : class
        {
            foreach (var serializer in Serializers)
            {
                if (serializer.CanDeserialize(restResponse))
                    return serializer.Deserialize<T>(restResponse);
            }

            throw new NotSupportedException("Cannot deserialize the response");
        }

        public static T Deserialize<T>(string raw) where T : class
        {
            foreach (var serializer in Serializers)
            {
                if (serializer.CanDeserialize(raw))
                    return serializer.Deserialize<T>(raw);
            }

            throw new NotSupportedException("Cannot deserialize the response");
        }

        public static string? Serialize<T>(DataFomat dataFormat, T? value) where T : class
        {
            if (value == null)
                return null;

            foreach (var serializer in Serializers)
            {
                switch (dataFormat)
                {
                    case DataFomat.Json:
                        {
                            if (serializer is IJsonSerializer)
                            {
                                return serializer.Serialize(value);
                            }
                            break;
                        }

                    case DataFomat.Xml:
                        {
                            if (serializer is IXmlSerializer)
                            {
                                return serializer.Serialize(value);
                            }
                            break;
                        }

                    default: throw new NotSupportedException($"The data format {dataFormat} is not supported in {nameof(SerializationManager)}.");
                }
            }

            throw new NotSupportedException($"Cannot serialize the object. The {nameof(SerializerCollection)} is empty.");
        }
    }
}
