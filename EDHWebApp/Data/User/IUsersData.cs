using EDHWebApp.Model;

namespace EDHWebApp.Data;

public interface IUsersData
{
    Task AddUser(User user);
    Task<IList<User>> GetAllUnregisteredUsers();
    Task RegisterUser(User user);
    Task<IList<User>> GetAllUsersByCompanyId(int companyId);

    Task RemoveUser(int userId);
    Task<User> GetUserById(string userId);
}