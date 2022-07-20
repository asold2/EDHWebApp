using Blazored.LocalStorage;
using EDHWebApp.Model;
using System.Text;
using System.Text.Json;

namespace EDHWebApp.Data.TokenData;

public class ITokenManagerImpl : ITokenManager
{
    private readonly ILocalStorageService _localStorageService;

    //private string uri = "http://localhost:5000";
    private string uri = "http://edhwebapi-dev.eu-north-1.elasticbeanstalk.com";
    private readonly HttpClient HttpClient;

    public ITokenManagerImpl(ILocalStorageService localStorage)
    {
        HttpClient = new HttpClient();
        _localStorageService = localStorage;

    }

    
    
    
    
    
    public async Task SetRefreshToke(string token)
    {
        await _localStorageService.SetItemAsync("refreshToken", token);

    }

    public async Task<string> GetRefreshToke()
    {
        string refreshToken = await _localStorageService.GetItemAsync<string>("refreshToken");
        return refreshToken;
    }

    public async Task<CompanyUser> getUserBasedOnRefreshToken()
    {
        Console.WriteLine("In sending message");
        
        TokenBody tokenBody = new TokenBody();
        tokenBody.RefreshToken = await GetRefreshToke();
        string tokenBodyAsJson = JsonSerializer.Serialize(tokenBody);
        HttpContent content = new StringContent(
            tokenBodyAsJson,
            Encoding.UTF8,
            "application/json"
        );

        var response = await HttpClient.PostAsync(uri + "/user/token/", content);
        Console.WriteLine("Sent request and received back");
        
        if (!response.IsSuccessStatusCode)
        {
            CompanyUser user = new CompanyUser();
            user.RefreshToken = "";
            return user;
            throw new Exception($"Error, {response.StatusCode}, {response.ReasonPhrase}");
            
        }
        
        var message = await response.Content.ReadAsStringAsync();

        Console.WriteLine("Here is the body        "  + message);

        

            
        var result = JsonSerializer.Deserialize<CompanyUser>(message);

        return result;
    }
}