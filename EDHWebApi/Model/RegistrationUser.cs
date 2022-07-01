using System.ComponentModel.DataAnnotations;

namespace EDHWebApi.Model;

public class RegistrationUser
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
    [DataType(DataType.EmailAddress, ErrorMessage = "Must be email format!")]
    public string Email { get; set; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "Full name is required")]
    public string FullName { get; set; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "Company name is required")]
    public string Company { get; set; }
}