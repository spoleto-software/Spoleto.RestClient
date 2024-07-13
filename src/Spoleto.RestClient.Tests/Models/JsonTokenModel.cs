using Spoleto.RestClient.Authentication.JWT;

namespace Spoleto.RestClient.Tests.Models
{
    /// <summary>
    /// The json token.
    /// </summary>
    public class JsonTokenModel : IJwtToken
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public JsonTokenModel()
        {
            Created = DateTime.UtcNow;
        }

        /// <summary>
        /// Gets the created date in UTC.
        /// </summary>
        public DateTime Created { get; }

        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the token type.
        /// </summary>
        public string TokenType { get; set; }

        /// <summary>
        /// Gets or sets the refresh token.
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Gets or sets the client code.
        /// </summary>
        public string ClientCode { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the issued date in UTC.
        /// </summary>
        public DateTime Issued { get; set; }

        /// <summary>
        /// Gets or sets the expires date in UTC.
        /// </summary>
        public DateTime Expires { get; set; }

        private TimeSpan? _deltaWeb;

        private TimeSpan GetDeltaWeb()
        {
            if (_deltaWeb == null)
            {
                _deltaWeb = Expires - Issued;

                // correct delta due to network lags (reduce it!)
                _deltaWeb = _deltaWeb.Value.Add(TimeSpan.FromMinutes(-2));
            }

            return _deltaWeb.Value;
        }

        /// <summary>
        /// Gets the is expires flag.
        /// </summary>
        /// <returns></returns>
        public bool IsExpires()
        {
            var delta = DateTime.UtcNow - Created;
            var deltaWeb = GetDeltaWeb();

            var compare = delta.CompareTo(deltaWeb);
            return compare > 0;
        }
    }
}
