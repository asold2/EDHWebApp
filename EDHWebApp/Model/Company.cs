using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace EDHWebApp.Model;

public class Company
{
    [JsonPropertyName("CompanyId"), Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CompanyId { get; set; }

    [Required (ErrorMessage = "A company must have a name!")] 
    [JsonPropertyName("Name")]
    public string Name { get; set; }
    
    [Required (ErrorMessage = "A company must have an email!")]
    [JsonPropertyName("Email")]
    [DataType(DataType.EmailAddress, ErrorMessage = "Must be email format!")]
    public string Email { get; set; }

    [JsonPropertyName("CreationDate")]
    public DateTime CreationDate { get; set; }
    
    
 

    public Company(string name, string email)
    {
        Name = name;
        Email = email;
        CreationDate = DateTime.Now;
    }

    public Company()
    {
    }
}