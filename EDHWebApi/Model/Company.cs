using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EDHWebApp.Model;

public class Company
{
    [JsonPropertyName("CompanyId"), Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CompanyId { get; set; }
    [JsonPropertyName("Name")]
    public string Name { get; set; }
    [JsonPropertyName("Email")]
    public string Email { get; set; }

    [JsonPropertyName("CreationDate")]
    public DateTime CreationDate { get; set; }

    public Company(string name, string email)
    {
        Name = name;
        Email = email;
        CreationDate = DateTime.Now;
    }
}