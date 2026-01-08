using System.ComponentModel.DataAnnotations;

namespace Asp.netcore_with_Angular.Model.Auth
{
    public class AssignRoleRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
