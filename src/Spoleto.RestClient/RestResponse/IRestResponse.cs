namespace Spoleto.RestClient
{
    public interface IRestResponse
    {
    }

    public interface IRestResponse<T> : IRestResponse
    {
        T Content { get; set; }
    }
}