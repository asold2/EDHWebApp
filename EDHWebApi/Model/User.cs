using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EDHWebApp.Model;

public abstract class User
{
    [JsonPropertyName("UserId"), Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserId { get; set; }
    [JsonPropertyName("Name")]
    public string Name { get; set; }
    [JsonPropertyName("Surname")]
    public string Surname { get; set; }
    [JsonPropertyName("Email")]
    public string Email { get; set; }
    [JsonPropertyName("MyCompany")]
    public Company MyCompany { get; set; }
    [JsonPropertyName("IsAdmin")]
    public bool IsAdmin { get; set; }
    [JsonPropertyName("VerifiedUser")]
    public bool VerifiedUser { get; set; }


    public User(string name, string surname, string email, Company myCompany, bool isAdmin, bool verifiedUser)
    {
        Name = name;
        Surname = surname;
        Email = email;
        this.MyCompany = myCompany;
        IsAdmin = isAdmin;
        VerifiedUser = verifiedUser;
    }

    protected User()
    {
        throw new NotImplementedException();
    }
}