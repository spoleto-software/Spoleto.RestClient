namespace Spoleto.RestClient.Serializers
{
    public interface ISerializer
    {
        bool CanDeserialize(IRestResponse restResponse);

        T Deserialize<T>(IRestResponse restResponse) where T : class;

        bool CanSerialize(RestRequest restRequest);

        string? Serialize<T>(RestRequest restRequest) where T : class;

        string? Serialize<T>(T? obj) where T : class;
    }
}
