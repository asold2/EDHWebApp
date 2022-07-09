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
    public async Task<IList<CompanyUser>> GetAllUsers()
    {
        try
        {
            IList<CompanyUser> users = await context.CompanyUsers.ToListAsync();
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
    public async Task<IList<CompanyUser>> GetUsersByCompany([FromRoute] int companyId)
    {
        try
        {
            IList<CompanyUser> usersToReturn = await context.CompanyUsers.Where(u => u.MyCompany.CompanyId == companyId).ToListAsync();
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
    public async Task<CompanyUser> GetUserByUserId([FromRoute] int userId)
    {
        Console.WriteLine(userId + "!!!!!!!!!!");
        
        
        CompanyUser companyUserToReturn =  await context.CompanyUsers.FirstOrDefaultAsync(u => u.UserId == userId);
        companyUserToReturn.TokenCreated = DateTime.Now;
        companyUserToReturn.TokenExpires = DateTime.Now;
        companyUserToReturn.MyCompany = await context.CustomerCompanies.FirstAsync(c => c.CompanyId == companyUserToReturn.CompanyId);
        return companyUserToReturn;
    }

 

}