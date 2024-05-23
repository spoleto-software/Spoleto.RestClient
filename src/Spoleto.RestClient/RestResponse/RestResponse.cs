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
    }
}
