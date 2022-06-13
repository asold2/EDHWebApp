using EDHWebApp.Model;
using EDHWebApp.Persistance;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EDHWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : Controller
{
    private EDHContext context;

    public UserController(EDHContext context)
    {
        this.context = context;
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


    [Route("/user/")]
    [HttpPost]
    public async Task<ActionResult<User>> AddUserAsync([FromBody] User user)
    {
        Console.WriteLine("In adding method with user " + user.Name);
        if (!ModelState.IsValid)
        {
            Console.WriteLine("bad request");

            return BadRequest(ModelState);
        }

        try
        {
            User existentUser = context.Users.FirstOrDefault(u => u.Email.Equals(user.Email));
            if (existentUser != null)
            {
                return StatusCode(403);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw e;
        }
        
        try
        {
            Company company = await context.Companies.FirstAsync(c => c.CompanyId == user.MyCompany.CompanyId);
            user.MyCompany = company;
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return Created($"/{user.UserId}", user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
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

    [Route("/user/removal/{userId:int}")]
    [HttpDelete]
    public async Task RemoveUserFromCompany([FromRoute] int userId)
    {
        User userToDelete = context.Users.FirstOrDefault(u => u.UserId == userId);
        try
        {
            context.Users.Remove(userToDelete);
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw e;
        }

    }

    [Route("/user/{userId:int}")]
    [HttpGet]
    public async Task<User> GetUserByUserId([FromRoute] int userId)
    {
        User userToReturn =  await context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        return userToReturn;
    }

    [Route("/registration/")]
    [HttpPut]
    public async Task<ActionResult<User>> RegisterUser([FromBody] User user)
    {

        Console.WriteLine("In registration method");
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            context.Update(user);
            await context.SaveChangesAsync();
            return   Accepted($"/{user.UserId}", user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw e;
        }
        
    }

}