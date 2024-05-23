using System.Net.Mime;

namespace Spoleto.RestClient
{
    /// <summary>
    /// Content types.
    /// </summary>
    public static class ContentTypes
    {
        /// <summary>
        /// "application/json"
        /// </summary>
        public const string ApplicationJson = "application/json";

        /// <summary>
        /// "text/json"
        /// </summary>
        public const string TextJson = "text/json";

        /// <summary>
        /// "text/plain"
        /// </summary>
        public const string TextPlain = "text/plain";

        /// <summary>
        /// "text/html"
        /// </summary>
        public const string TextHtml = "text/html";

        /// <summary>
        /// "application/json-patch+json"
        /// </summary>
        public const string ApplicationJsonPatch = "application/json-patch+json";

        /// <summary>
        /// "application/*+json"
        /// </summary>
        public const string ApplicationAnyJsonSyntax = "application/*+json";

        /// <summary>
        /// "application/x-msgpack"
        /// </summary>
        public const string MessagePack = "application/x-msgpack";

        /// <summary>
        /// "application/x-lz4msgpack"
        /// </summary>
        public const string LZ4MessagePack = "application/x-lz4msgpack";

        /// <summary>
        /// "application/xml"
        /// </summary>
        public const string ApplicationXml = "application/xml";

        /// <summary>
        /// "application/x-www-form-urlencoded"
        /// </summary>
        public const string ApplicatioFormUrlEncoded = "application/x-www-form-urlencoded";

        /// <summary>
        /// ApplicationOctetStream
        /// </summary>
        public const string ApplicationOctetStream = "application/octet-stream";
    }
}
