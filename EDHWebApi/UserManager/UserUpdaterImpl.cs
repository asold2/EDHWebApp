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
            User requestingUser = await _edhContext.Users.FirstAsync(u => u.UserId == userId);
            requestingUser.NumberOfRequests++;
            _edhContext.Update(requestingUser);
            await _edhContext.SaveChangesAsync();

        }
        catch (InvalidOperationException e)
        {
            throw e;
        }
    }

    public async Task RegisterUser(User user)
    {
        _edhContext.Update(user);
        await _edhContext.SaveChangesAsync();
    }
}