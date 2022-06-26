using System.Xml;
using EDHWebApp.Model;
using EDHWebApp.Persistance;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EDHWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class RegisteredUserController : Controller
{
    private EDHContext context;

    public RegisteredUserController(EDHContext context)
    {
        this.context = context;
    }

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
            // User tempUser = context.Users.FirstOrDefault(u => u.UserId == user.UserId);
            

            
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

    [Route("/account")]
    [HttpPost]
    public async Task<User> AuthenticateUser([FromBody] User user)
    {

        Console.WriteLine(user.UserName );
        Console.WriteLine(user.Password );

        if (user.UserName != "string" && user.Password != "string")
        {
            try
            {
                User userToReturn =
                    await context.Users.FirstAsync(u => u.UserName == user.UserName && u.Password == user.Password);
                return userToReturn;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        return null;



    }

}