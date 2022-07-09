using EDHWebApp.Model;

namespace EDHWebApp.Data;

public interface IUsersData
{
    Task AddUser(CompanyUser companyUser);
    Task<IList<CompanyUser>> GetAllUnregisteredUsers();
    Task RegisterUser(Account account);
    Task<IList<CompanyUser>> GetAllUsersByCompanyId(int companyId);

    Task RemoveUser(int userId);
    Task<CompanyUser> GetUserById(string userId);
}