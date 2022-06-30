using System.Net;
using System.Net.Mail;
using System.Xml;
using EDHWebApi.Model;
using EDHWebApi.EmailSender;
using EDHWebApi.Persistance;
using EDHWebApi.UserManager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace EDHWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class RegisteredUserController : Controller
{
    private EmailSender.EmailSender _emailSender;
    private EDHContext context;
    private UserUpdater _userUpdater;

    public RegisteredUserController(EDHContext context)
    {
        this.context = context;
        _emailSender = new EmailSenderImpl();
        _userUpdater = new UserUpdaterImpl(context);
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
                Console.WriteLine(userToReturn.MyCompany + "!!!!!!!");
                Console.WriteLine(userToReturn.Name + "@@@@@@@@@@@");
                
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

        User user = await context.Users.FirstAsync(u => u.UserId == pictureEmail.userId);
        Console.WriteLine(user.MyCompany+ "!!!!!!!!!!!!!!!!!!!!");
        Console.WriteLine(user.Email+ "!!!!!!!!!!!!!!!!!!!!");
        Console.WriteLine(user.Role+ "!!!!!!!!!!!!!!!!!!!!");
        Console.WriteLine(user.Surname+ "!!!!!!!!!!!!!!!!!!!!");
        // Company company = await context.Companies.FirstAsync(c=>c.CompanyId==user.);       


        try
        {
            _userUpdater.UpdateUserNumberOfRequests(pictureEmail.userId);

            // _emailSender.SendPictureFromUserToCompany(pictureEmail, user);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
            throw;
        }
        
    }
}