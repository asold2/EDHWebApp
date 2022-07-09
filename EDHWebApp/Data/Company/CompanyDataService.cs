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


    public async Task RegisterCompany(CustomerCompany customerCompany)
    {
        string companyAsJson = JsonSerializer.Serialize(customerCompany, new JsonSerializerOptions
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
    
    
    
    

    public async Task<IList<CustomerCompany>> GetAllCompanies()
    {
        string receivedMessage = await HttpClient.GetStringAsync(uri + "/companies/");
        IList<CustomerCompany> companies = JsonSerializer.Deserialize<IList<CustomerCompany>>(receivedMessage);
        return companies;
    }

    public async Task DeleteCompany(int companyId)
    {
        await HttpClient.DeleteAsync(uri + $"/company/{companyId}");

    }

    public async Task<CustomerCompany> getCompanyByCompanyEmail(string email)
    {
        string receivedMessage = await HttpClient.GetStringAsync(uri + $"/company/{email}");
        CustomerCompany customerCompany = JsonSerializer.Deserialize<CustomerCompany>(receivedMessage);
        return customerCompany;

    }
}