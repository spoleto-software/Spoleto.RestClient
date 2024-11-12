using System.Text;

namespace Spoleto.RestClient
{
    public record RestClientOptions
    {
        public static readonly RestClientOptions Default = new();

        public string ContentType { get; set; } = Spoleto.RestClient.ContentTypes.ApplicationJson;

        public Encoding Encoding { get; set; } = Encoding.UTF8;

        /// <summary>
        /// Gets or sets the flag indicating whether throw an exception if <see cref="System.Net.HttpStatusCode.NotFound"/> is received.
        /// </summary>
        /// <remarks>
        /// Default value: false.
        /// </remarks>
        public bool ThrowExceptionIfNotFound { get; set; } = false;
    }
}
