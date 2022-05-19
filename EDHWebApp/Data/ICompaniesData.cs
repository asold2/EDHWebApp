using EDHWebApp.Model;

namespace EDHWebApp.Data;

public interface ICompaniesData
{
    Task RegisterCompany(Company company);
    Task<IList<Company>> GetAllCompanies();

}