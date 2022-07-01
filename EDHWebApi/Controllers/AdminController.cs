using EDHWebApi.Model;
using EDHWebApi.Persistance;
using EDHWebApi.UserManager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EDHWebApi.Controllers;


[ApiController]
[Route("[controller]")]

public class AdminController : Controller
{
    private EDHContext context;
    private UserUpdater _userUpdater;

    public AdminController(EDHContext context)
    {
        this.context = context;
        _userUpdater = new UserUpdaterImpl(context);
    }
    
    [Route("/user/")]
    [HttpPost]
    public async Task<ActionResult<User>> AddUserAsync([FromBody] User user)
    {

        
        if (!ModelState.IsValid)
        {
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
            user.CompanyId = company.CompanyId;
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
    
    
    [Route("/registration/")]
    [HttpPut]
    public async Task<ActionResult<User>> RegisterUser([FromBody] User user)
    {

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            _userUpdater.RegisterUser(user);
            // context.Update(user);
            // await context.SaveChangesAsync();
            return   Accepted($"/{user.UserId}", user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw e;
        }
        
    }
    
    
    //The unregistered user now has username and password
    [Route("/new/registration/")]
    [HttpPost]
    public async Task<ActionResult<User>> AddRegisteredUser([FromBody] User user)
    {
        Console.WriteLine(user.UserName + user.Password + "AAAAAAAA");
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            

            
            context.Users.Update(user);
            await context.SaveChangesAsync();
            return Created($"/{user.UserId}", user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }

    }
    
    
   

}