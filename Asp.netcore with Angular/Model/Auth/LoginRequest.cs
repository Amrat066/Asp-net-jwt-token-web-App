using System.ComponentModel.DataAnnotations;

namespace Asp.netcore_with_Angular.Model.Auth
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
