using System.ComponentModel.DataAnnotations;

namespace Spoleto.RestClient.Tests.Models
{
    public class RefreshTokenModel
    {
        /// <summary>
        /// Gets or sets the client code.
        /// </summary>
        [Required(ErrorMessage = "Client code is not specified.")]
        public string ClientCode { get; set; }

        /// <summary>
        /// Gets or sets the client secret.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the refresh token.
        /// </summary>
        [Required(ErrorMessage = "Refresh token is not specified.")]
        public string RefreshToken { get; set; }
    }
}
