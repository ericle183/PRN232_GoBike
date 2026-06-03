using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.DTOs;

public class UpdateStaffUserRequest
{
    [Required]
    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [EmailAddress]
    [MaxLength(100)]
    public string? Email { get; set; }

    [MinLength(6)]
    public string? Password { get; set; }

    public bool IsActive { get; set; } = true;
}
