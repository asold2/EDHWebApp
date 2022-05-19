using System.Text;
using System.Text.Json;
using EDHWebApp.Model;

namespace EDHWebApp.Data;

public class CompanyDataService : ICompaniesData
{
    private string uri = "https://localhost:7213";
    private readonly HttpClient HttpClient;

    public CompanyDataService()
    {
        HttpClient = new HttpClient();
    }


    public async Task RegisterCompany(Company company)
    {
        string companyAsJson = JsonSerializer.Serialize(company, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        HttpContent content = new StringContent(
            companyAsJson,
            Encoding.UTF8,
            "application/json"
        );

        await HttpClient.PostAsync(uri + "/new/company/", content);
    }

    public Task<IList<Company>> GetAllCompanies()
    {
        throw new NotImplementedException();
    }
}