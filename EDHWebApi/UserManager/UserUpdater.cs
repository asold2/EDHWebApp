using EDHWebApi.Model;

namespace EDHWebApi.UserManager;

public interface UserUpdater
{

    
    
    
    Task UpdateUserNumberOfRequests(int userId);
    Task RegisterUser(CompanyUser companyUser);
}