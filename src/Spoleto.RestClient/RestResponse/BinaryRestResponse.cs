namespace Spoleto.RestClient
{
    public record BinaryRestResponse : RestResponse<byte[]>, IBinaryRestResponse
    {
        public BinaryRestResponse()
        {
        }

        public BinaryRestResponse(RestResponse<byte[]> content) : base(content)
        {
        }
    }
}
