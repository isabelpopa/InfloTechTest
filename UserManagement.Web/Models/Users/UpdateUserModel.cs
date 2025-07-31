using System;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Web.Models.Users;

public class UpdateUserModel
{
    public long Id { get; set; }

    [Required(ErrorMessage = "Forename is required")]
    public string Forename { get; set; } = default!;

    [Required(ErrorMessage = "Surname is required")]
    public string Surname { get; set; } = default!;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Email must be valid")]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = "Date of birth is required")]
    [DataType(DataType.Date, ErrorMessage = "Date of birth must be between 1900 and 2025")]
    [Range(typeof(DateOnly), "1900-01-01", "2025-07-31", ErrorMessage = "Date of birth must be between 1900 and 2025")]
    public DateOnly DateOfBirth { get; set; }

    public bool IsActive { get; set; }
}
