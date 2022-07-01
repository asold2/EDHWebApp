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

   

    //User Authentication
    [Route("/account")]
    [HttpPost]
    public async Task<User> AuthenticateUser([FromBody] User user)
    {
        Console.WriteLine(user.Password);
        Console.WriteLine(user.UserName);
        
        
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

        User user = await context.Users.FirstAsync(u => u.UserId == pictureEmail.userId);
        var companyId = context.Users.FromSqlRaw("select MyCompanyCompanyId from Users where UserId = {0}", user.UserId);
        Company usersCompany = await context.Companies.FirstAsync(c => c.CompanyId == user.CompanyId);
        Console.WriteLine(usersCompany.Email + "!!!!!!!!!!!!!!!");


        try
        {
            _userUpdater.UpdateUserNumberOfRequests(pictureEmail.userId);

            _emailSender.SendPictureFromUserToCompany(pictureEmail, user, usersCompany.Email);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
            throw;
        }
        
    }
}