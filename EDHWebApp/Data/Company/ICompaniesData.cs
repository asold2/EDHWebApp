using EDHWebApp.Model;

namespace EDHWebApp.Data;

public interface ICompaniesData
{
    Task RegisterCompany(CustomerCompany customerCompany);
    Task<IList<CustomerCompany>> GetAllCompanies();

    Task DeleteCompany(int companyId);

    Task<CustomerCompany> getCompanyByCompanyEmail(string email);
}