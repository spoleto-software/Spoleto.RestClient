using System.ComponentModel.DataAnnotations;

namespace Spoleto.RestClient.Tests.Models
{
    /// <summary>
    /// The login model for database authentication based on login/password.
    /// </summary>
    public class LoginModelCredentials : LoginModelBase
    {
        /// <summary>
        /// Gets or sets the user password.
        /// </summary>
        [Required(ErrorMessage = "Password is not specified.")]
        public string Password { get; set; }
    }
}
