using System.ComponentModel.DataAnnotations;

namespace AspNetSecurityApplication.Models;

public class AuthRequest : IValidatableObject
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [StringLength(100)]
    public string? FullName { get; set; }

    public bool IsRegister { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (IsRegister && string.IsNullOrWhiteSpace(FullName))
        {
            yield return new ValidationResult("Full name is required.", new[] { nameof(FullName) });
        }
    }
}
