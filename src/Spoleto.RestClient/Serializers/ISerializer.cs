namespace Spoleto.RestClient.Serializers
{
    public interface ISerializer
    {
        bool CanDeserialize(IRestResponse restResponse);

        bool CanDeserialize(string raw);

        T Deserialize<T>(IRestResponse restResponse) where T : class;

        T Deserialize<T>(string raw) where T : class;

        string? Serialize<T>(T? obj) where T : class;
    }
}
