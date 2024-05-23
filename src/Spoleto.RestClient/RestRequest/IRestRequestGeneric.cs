namespace Spoleto.RestClient
{
    public interface IRestRequestGeneric<T> : IRestRequest where T : class
    {
        T? Content { get; set; }
    }
}