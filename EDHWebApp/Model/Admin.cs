using System.Text.Json.Serialization;

namespace EDHWebApp.Model;

public class Admin : User
{
    [JsonPropertyName("Username")]
    public string Username { get; set; }
    [JsonPropertyName("Password")]
    public string Password { get; set; }


    public Admin()
    {
    }

    public Admin( string username, string password) : base()
    {
        Username = username;
        Password = password;
    }
}