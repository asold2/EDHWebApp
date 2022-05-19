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


    [Route("/new/user/")]
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
            throw;
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



}