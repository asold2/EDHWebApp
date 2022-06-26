using System.Text;
using System.Text.Json;
using EDHWebApp.Model;
using Microsoft.AspNetCore.Mvc;

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

    public async Task<IList<Company>> GetAllCompanies()
    {
        string receivedMessage = await HttpClient.GetStringAsync(uri + "/companies/");
        IList<Company> companies = JsonSerializer.Deserialize<IList<Company>>(receivedMessage);
        return companies;
    }

    public async Task DeleteCompany(int companyId)
    {
        Console.WriteLine("Sending request to delete company 1");
        await HttpClient.DeleteAsync(uri + $"/company/{companyId}");
        Console.WriteLine("Sending request to delete company 2");

    }
}