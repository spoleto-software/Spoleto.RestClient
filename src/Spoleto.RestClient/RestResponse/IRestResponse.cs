using System.Net;

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
        /// Gets or sets the value of the Content-Type content header on an HTTP response.
        /// </summary>
        string? ContentType { get; set; }
    }

    public interface IRestResponse<T> : IRestResponse
    {
        T Content { get; set; }
    }
}