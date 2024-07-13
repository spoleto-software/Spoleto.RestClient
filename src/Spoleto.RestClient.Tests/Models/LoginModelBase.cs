using System.ComponentModel.DataAnnotations;

namespace Spoleto.RestClient.Tests.Models
{
    /// <summary>
    /// The base login model.
    /// </summary>
    public abstract class LoginModelBase
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
        /// Gets or sets the login type.
        /// </summary>
        [Required(ErrorMessage = "Login type is not specified.")]
        public string LoginType { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        [Required(ErrorMessage = "UserName is not specified.")]
        public string UserName { get; set; }
    }
}
