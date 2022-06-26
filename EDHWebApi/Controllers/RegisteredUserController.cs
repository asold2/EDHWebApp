using System.Net;
using System.Net.Mail;
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
    public async Task SendEmailForRegistration([FromBody] RegistrationUser regUser)
    {
        Console.WriteLine("received req for registration in API");
        string messageToSend = "I, " + regUser.FullName + ", would like to request an account. I work at " + regUser.Company + ".\n My email is " + regUser.Email;
        try
        {
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            message.From = new MailAddress("edhtech2022@gmail.com");
            message.To.Add(new MailAddress("asoldan1459@gmail.com"));
            message.Subject = "Request Account";
            message.IsBodyHtml = true;
            message.Body = messageToSend;
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com";
            smtp.UseDefaultCredentials = false;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential("edhtech2022@gmail.com", "qlxsksjccfqtskyj");
            
            
            smtp.Send(message);
         
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
    }
}