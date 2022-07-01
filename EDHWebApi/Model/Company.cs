using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace EDHWebApi.Model;

public class Company
{
    [JsonPropertyName("CompanyId"), Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CompanyId { get; set; }
    [JsonPropertyName("Name")]
    public string Name { get; set; }
    [JsonPropertyName("Email")]
    [DataType(DataType.EmailAddress)]

    public string Email { get; set; }

    [JsonPropertyName("CreationDate")]
    public DateTime CreationDate { get; set; }

   //  [JsonPropertyName("Employees")]
   //  [AllowNull]
   // [JsonIgnore]
   //  public ICollection<User> Employees { get; }



    public Company(string name, string email)
    {
        Name = name;
        Email = email;
        CreationDate = DateTime.Now;
    }
}