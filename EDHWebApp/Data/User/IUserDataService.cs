using System.Text;
using System.Text.Json;
using EDHWebApp.Model;

namespace EDHWebApp.Data;

public class IUserDataService : IUsersData
{
    
    private string uri = "https://localhost:7213";
    private readonly HttpClient HttpClient;

    public IUserDataService()
    {
        HttpClient = new HttpClient();
    }

    
    
    
    public async  Task AddUser(User user)
    {
        string userAsJson = JsonSerializer.Serialize(user);
        HttpContent content = new StringContent(
            userAsJson,
            Encoding.UTF8,
            "application/json"
        );

        await HttpClient.PostAsync(uri + "/user/", content);
        Console.WriteLine(userAsJson);
    }

    public async Task<IList<User>> GetAllUnregisteredUsers()
    {
        string receivedMessage = await HttpClient.GetStringAsync(uri + "unreg/users/");
        IList<User> UnregUsers = JsonSerializer.Deserialize<IList<User>>(receivedMessage);
        return UnregUsers;
    }

    public async Task RegisterUser(User user)
    {
        
        string userAsJson = JsonSerializer.Serialize(user, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        HttpContent content = new StringContent(
            userAsJson,
            Encoding.UTF8,
            "application/json"
        );

        
        await HttpClient.PutAsync(uri + "/registration/", content);
        Console.WriteLine("registering user");
    }

    public async Task<IList<User>> GetAllUsersByCompanyId(int companyId)
    {
        string receivedMessage = await HttpClient.GetStringAsync(uri + $"/users/{companyId}");
        IList<User> companyUsers = JsonSerializer.Deserialize<IList<User>>(receivedMessage);
        return companyUsers;
    }

    public async Task RemoveUser(int userId)
    {
        await HttpClient.DeleteAsync(uri + $"/user/removal/{userId}");
        Console.WriteLine("Removing user");
    }

    public async Task<User> GetUserById(string userId)
    {
        string receivedMessage = await HttpClient.GetStringAsync(uri + $"/user/{userId}");
        User searchedUser = JsonSerializer.Deserialize<User>(receivedMessage);

        return searchedUser;



    }
}