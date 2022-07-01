using EDHWebApi.Model;
using EDHWebApi.Persistance;
using EDHWebApi.UserManager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EDHWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : Controller
{
    private EDHContext context;
    private UserUpdater _userUpdater;

    public UserController(EDHContext context)
    {
        this.context = context;
        _userUpdater = new UserUpdaterImpl(context);
    }

    [Route("/users/")]
    [HttpGet]
    public async Task<IList<User>> GetAllUsers()
    {
        try
        {
            IList<User> users = await context.Users.ToListAsync();
            return users;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }


    

    [Route("/users/{companyId:int}")]
    [HttpGet]
    public async Task<IList<User>> GetUsersByCompany([FromRoute] int companyId)
    {
        try
        {
            IList<User> usersToReturn = await context.Users.Where(u => u.MyCompany.CompanyId == companyId).ToListAsync();
            return usersToReturn;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }



    [Route("/user/{userId:int}")]
    [HttpGet]
    public async Task<User> GetUserByUserId([FromRoute] int userId)
    {
        User userToReturn =  await context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        return userToReturn;
    }

 

}