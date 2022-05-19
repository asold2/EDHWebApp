using System.Text.Json.Serialization;

namespace EDHWebApp.Model;

public class RegisteredUser : User
{
    [JsonPropertyName("Username")]
    public string Username { get; set; }
    [JsonPropertyName("Password")]
    public string Password { get; set; }

    public RegisteredUser()
    {
    }

    public RegisteredUser(string username, string password) : base()
    {
        Username = username;
        Password = password;
    }
}