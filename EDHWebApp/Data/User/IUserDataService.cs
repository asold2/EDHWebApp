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

    
    
    
    public async  Task AddUser(CompanyUser companyUser)
    {
        companyUser.CompanyId = companyUser.MyCustomerCompany.CompanyId;
        string userAsJson = JsonSerializer.Serialize(companyUser);
        HttpContent content = new StringContent(
            userAsJson,
            Encoding.UTF8,
            "application/json"
        );

        await HttpClient.PostAsync(uri + "/user/", content);
    }

    public async Task<IList<CompanyUser>> GetAllUnregisteredUsers()
    {
        string receivedMessage = await HttpClient.GetStringAsync(uri + "unreg/users/");
        IList<CompanyUser> UnregUsers = JsonSerializer.Deserialize<IList<CompanyUser>>(receivedMessage);
        return UnregUsers;
    }

    public async Task RegisterUser(Account account)
    {
        
        string userAsJson = JsonSerializer.Serialize(account, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        HttpContent content = new StringContent(
            userAsJson,
            Encoding.UTF8,
            "application/json"
        );

        
        await HttpClient.PutAsync(uri + "/registration/", content);
    }

    public async Task<IList<CompanyUser>> GetAllUsersByCompanyId(int companyId)
    {
        string receivedMessage = await HttpClient.GetStringAsync(uri + $"/users/{companyId}");
        IList<CompanyUser> companyUsers = JsonSerializer.Deserialize<IList<CompanyUser>>(receivedMessage);
        return companyUsers;
    }

    public async Task RemoveUser(int userId)
    {
        await HttpClient.DeleteAsync(uri + $"/user/removal/{userId}");
    }

    public async Task<CompanyUser> GetUserById(string userId)
    {
        string receivedMessage = await HttpClient.GetStringAsync(uri + $"/user/{userId}");
        CompanyUser searchedCompanyUser =  JsonSerializer.Deserialize<CompanyUser>(receivedMessage);
        
        return searchedCompanyUser;
    }
}