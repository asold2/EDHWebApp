using EDHWebApi.Model;
using EDHWebApi.Persistance;
using Microsoft.EntityFrameworkCore;

namespace EDHWebApi.UserManager;

public class UserUpdaterImpl : UserUpdater
{
    
    private EDHContext _edhContext;

    public UserUpdaterImpl(EDHContext edhContext)
    {
        _edhContext = edhContext;
    }


    public async Task UpdateUserNumberOfRequests(int userId)
    {
        try
        {
            CompanyUser requestingCompanyUser = await _edhContext.CompanyUsers.FirstAsync(u => u.UserId == userId);
            requestingCompanyUser.NumberOfRequests++;
            _edhContext.Update(requestingCompanyUser);
            await _edhContext.SaveChangesAsync();

        }
        catch (InvalidOperationException e)
        {
            throw e;
        }
    }

    public async Task RegisterUser(CompanyUser companyUser)
    {
        _edhContext.Update(companyUser);
        await _edhContext.SaveChangesAsync();
    }
}