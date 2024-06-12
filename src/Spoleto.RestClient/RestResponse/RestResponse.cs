using System.Net;
using System.Text;

namespace Spoleto.RestClient
{
    public abstract record RestResponse<TContent> : IRestResponse<TContent>
    {
        public RestResponse()
        {
        }

        public RestResponse(TContent content)
        {
            Content = content;
        }

        public TContent Content { get; set; }

        public virtual Encoding Encoding { get; set; } = Encoding.UTF8;

        /// <summary>
        /// Gets or sets a value that indicates if the HTTP response was successful.
        /// </summary>
        public bool IsSuccessStatusCode { get; set; }

        /// <summary>
        /// Gets or sets the status code of the HTTP response.
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the value of the Content-Type content header on an HTTP response.
        /// </summary>
        public string? ContentType { get; set; }
    }
}
