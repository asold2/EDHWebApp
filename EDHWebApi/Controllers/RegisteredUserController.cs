using System.Xml;
using EDHWebApp.Model;
using EDHWebApp.Persistance;
using Microsoft.AspNetCore.Mvc;

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

    [Route("/new/registration/{userName}/{password}")]
    [HttpPost]
    public async Task<ActionResult<RegisteredUser>> AddRegisteredUser([FromBody] User user, [FromRoute] string userName,
        [FromRoute] string password)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            User tempUser = context.Users.FirstOrDefault(u => u.UserId == user.UserId);
            // RegisteredUser ru = new RegisteredUser
            // {
            //     Username = userName,
            //     Password = password,
            //     UserId = tempUser.UserId,
            //     Name = tempUser.Name,
            //     Surname = tempUser.Surname,
            //     Email = tempUser.Email,
            //     IsAdmin = tempUser.IsAdmin,
            //     MyCompany = tempUser.MyCompany,
            //     VerifiedUser = tempUser.VerifiedUser
            // };
            RegisteredUser ru = new RegisteredUser(userName, password);
            context.Users.Remove(tempUser);

            ru.GetFromParent(tempUser);
            ru.UserId = tempUser.UserId;
            Console.WriteLine(ru.Password + ru.Username + "!@!@!@");

            Console.WriteLine(ru.Email+"!!!!" + ru.UserId);
            await context.RegisteredUsers.AddAsync(ru);
            // await context.SaveChangesAsync();
            return Created($"/{ru.UserId}", ru);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }

    }

}