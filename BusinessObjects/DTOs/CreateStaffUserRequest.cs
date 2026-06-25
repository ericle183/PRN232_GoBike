using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.DTOs;

public class CreateStaffUserRequest
{
    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [EmailAddress]
    [MaxLength(100)]
    public string? Email { get; set; }

    public bool IsActive { get; set; } = true;
}
