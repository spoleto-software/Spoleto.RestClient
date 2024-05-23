using System.Text;

namespace Spoleto.RestClient
{
    public record RestClientOptions
    {
        public static readonly RestClientOptions Default = new();

        public string ContentType { get; set; } = Spoleto.RestClient.ContentTypes.ApplicationJson;

        public Encoding Encoding { get; set; } = Encoding.UTF8;
    }
}
