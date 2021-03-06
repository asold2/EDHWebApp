using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace EDHWebApp.Model;

public class CompanyUser
{
    [JsonPropertyName("UserId"), Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserId { get; set; }
    
    [Required (ErrorMessage = "A user must have a name!")]
    [JsonPropertyName("Name")]
    public string Name { get; set; }
    
    [Required (ErrorMessage = "A user must have a surname!")]
    [JsonPropertyName("Surname")]
    public string Surname { get; set; }
    
    [Required (ErrorMessage = "A user must have an email!")]
    [JsonPropertyName("Email")]
    [DataType(DataType.EmailAddress, ErrorMessage = "Must be email format!")]
    public string Email { get; set; }

    [JsonPropertyName("MyCompany")]

    public CustomerCompany MyCustomerCompany { get; set; }
    
    
    
    [JsonPropertyName("IsAdmin")]
    public bool IsAdmin { get; set; }
    [JsonPropertyName("VerifiedUser")] 
    public bool VerifiedUser { get; set; }
    
    [JsonPropertyName("Role")]
    public string Role { get; set; }
    [JsonPropertyName("username")]

    [JsonIgnore]

    public string? Username { get; set; } = string.Empty;

    [JsonIgnore]
    public byte[]? PasswordHash { get; set; }
    [JsonIgnore]

    public byte[]? PasswordSalt { get; set; }
    // [JsonIgnore]
    [JsonPropertyName("RefreshToken")]


    public string? RefreshToken { get; set; } = string.Empty;
    // [JsonIgnore]
    [JsonPropertyName("TokenCreated")]

    public DateTime? TokenCreated { get; set; }

    // [JsonIgnore]
    [JsonPropertyName("TokenExpires")]

    public DateTime? TokenExpires { get; set; }

    public int CompanyId { get; set; }


    [JsonPropertyName("NumberOfRequests")]
    public int NumberOfRequests { get; set; }

    [JsonPropertyName("RememberMe")]
    public bool RememberMe { get; set; }

}