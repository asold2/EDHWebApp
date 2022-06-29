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


    public User(string name, string surname, string email, Company myCompany, bool isAdmin, bool verifiedUser)
    {
        Name = name;
        Surname = surname;
        Email = email;
        this.MyCompany = myCompany;
        IsAdmin = isAdmin;
        VerifiedUser = verifiedUser;
        Role = "User";
    }

    public User(string userName, string password)
    {
        this.Password = password;
        this.UserName = userName;
    }


    [JsonConstructor]
    public User()
    {
        
    }
}