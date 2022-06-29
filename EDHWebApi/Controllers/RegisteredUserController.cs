using System.Net;
using System.Net.Mail;
using System.Xml;
using EDHWebApi.Model;
using EDHWebApi.EmailSender;
using EDHWebApi.Persistance;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EDHWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class RegisteredUserController : Controller
{
    private EmailSender.EmailSender _emailSender;
    private EDHContext context;

    public RegisteredUserController(EDHContext context)
    {
        this.context = context;
        _emailSender = new EmailSenderImpl();
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

    //User Authentication
    [Route("/account")]
    [HttpPost]
    public async Task<User> AuthenticateUser([FromBody] User user)
    {
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

    [HttpPost]
    [Route("/reg/email")]
    public async Task<ActionResult> SendEmailForRegistration([FromBody] RegistrationUser regUser)
    {
        try
        {
            _emailSender.SendRegistrationEmail(regUser);
            return Ok();
        }
        catch (Exception e)
        {
            return NotFound();
            throw;
        }

    }


    [HttpPost]
    [Route("/picture")]
    public async Task<ActionResult> ReceivePictureFromUser([FromBody] PictureEmail pictureEmail)
    {
        try
        {
            _emailSender.SendPictureFromUserToCompany(pictureEmail);
            return Ok();
        }
        catch (Exception e)
        {
            return NotFound();
            throw;
        }
        
    }
}