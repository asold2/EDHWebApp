using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EDHWebApi.Model;

public class User 
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
    public Company MyCompany { get; set; }
    
    
    
    [JsonPropertyName("IsAdmin")]
    public bool IsAdmin { get; set; }
    [JsonPropertyName("VerifiedUser")] 
    public bool VerifiedUser { get; set; }
    
    [JsonPropertyName("Role")]
    public string Role { get; set; }
    [JsonPropertyName("username")]

    public string UserName { get; set; }
    [JsonPropertyName("password")]
 

    public string Password { get; set; }

    public int CompanyId { get; set; }


    [JsonPropertyName("NumberOfRequests")]
    public int NumberOfRequests { get; set; }

    public User(string Name, string Surname, string Email,  bool isAdmin, bool verifiedUser, string Role, string userName, string password, int companyId, int numberOfRequests)
    {
        this.Name = Name;
        this.Surname = Surname;
        this.Email = Email;
        this.IsAdmin = isAdmin;
        this.VerifiedUser = verifiedUser;
        this.Role = Role;
        this.UserName = userName;
        this.Password = password;
        this.CompanyId = companyId;
        this.NumberOfRequests = numberOfRequests;
    }
}