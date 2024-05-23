using System.Text;

namespace Spoleto.RestClient
{
    public record TextRestResponse : RestResponse<string>, ITextRestResponse
    {
        public TextRestResponse()
        {
        }

        public TextRestResponse(string content) : base(content)
        {
        }
    }
}
