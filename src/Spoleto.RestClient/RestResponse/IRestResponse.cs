using System.Net;
using System.Text;

namespace Spoleto.RestClient
{
    public interface IRestResponse
    {
        /// <summary>
        /// Gets or sets the value that indicates if the HTTP response was successful.
        /// </summary>
        bool IsSuccessStatusCode { get; set; }

        /// <summary>
        /// Gets or sets the status code of the HTTP response.
        /// </summary>
        HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the value of the MediaType from Content-Type content header on an HTTP response.
        /// </summary>
        string? ContentType { get; set; }

        /// <summary>
        /// Gets or sets the value of the Encoding from Content-Type content header on an HTTP response or default UTF-8.
        /// </summary>
        Encoding Encoding { get; set; }

        /// <summary>
        /// Gets the content.
        /// </summary>
        object? GetContent();
    }

    public interface IRestResponse<T> : IRestResponse
    {
        T Content { get; set; }
    }
}