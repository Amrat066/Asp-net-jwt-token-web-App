using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetSecurityApplication.Models;

public class ApplicationUser
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [NotMapped]
    [StringLength(100, MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
