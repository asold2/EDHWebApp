using System.Text.Json.Serialization;

namespace EDHWebApp.Model;

public class Account
{
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;
    [JsonPropertyName("password")]

    public string Password { get; set; } = string.Empty;

    public int userId { get; set; }
    public bool rememberMe { get; set; }
}