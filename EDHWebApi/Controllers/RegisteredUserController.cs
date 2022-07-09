using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Xml;
using EDHWebApi.Model;
using EDHWebApi.EmailSender;
using EDHWebApi.Persistance;
using EDHWebApi.UserManager;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SQLitePCL;


namespace EDHWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class RegisteredUserController : Controller
{
    private EmailSender.EmailSender _emailSender;
    private EDHContext context;
    private UserUpdater _userUpdater;
    private readonly IConfiguration _configuration;

    public RegisteredUserController(EDHContext context, IConfiguration configuration)
    {
        this.context = context;
        _emailSender = new EmailSenderImpl();
        _userUpdater = new UserUpdaterImpl(context);
        _configuration = configuration;
    }

   

    //User Authentication
    [Route("/account")]
    [HttpPost]
    public async Task<ActionResult<CompanyUser>> AuthenticateUser([FromBody] Account user)
    {
        bool found = false;
        CompanyUser loggingCompanyUser = null;
        List<CompanyUser> users = await context.CompanyUsers.ToListAsync();
        foreach(CompanyUser u in users)
        {
            if (u.Username == user.Username)
            {
                found = true;
            }
        }

        if (found == false)
        {
            return BadRequest("User not found");
        }
        else if(found == true)
        {
            loggingCompanyUser = await context.CompanyUsers.FirstAsync(u => u.Username == user.Username);
        }

        if (!VerifyPasswordHash(user.Password, loggingCompanyUser.PasswordHash, loggingCompanyUser.PasswordSalt))
        {
            return BadRequest("Wrong password.");
        }

        
        string token = CreateToken(loggingCompanyUser);
        var refreshToken = GenerateRefreshToken();
        SetRefreshToken(refreshToken, loggingCompanyUser, user.RememberMe);
        
        Thread.Sleep(500);
        
        CustomerCompany company = await context.CustomerCompanies.FirstAsync(c => c.CompanyId == loggingCompanyUser.CompanyId);
        loggingCompanyUser.MyCompany = company;
        

        string isVerified = "";
        if (loggingCompanyUser.VerifiedUser)
        {
            isVerified = "isVerified";
        }
        
        
        var claims = new List<Claim>();
        
        claims.Add(new Claim(ClaimTypes.Role, loggingCompanyUser.Role));
        claims.Add(new Claim("isVerified", isVerified));
        claims.Add(new Claim(ClaimTypes.Email, loggingCompanyUser.Email));
        //Add a claim to check for the type of the subclass(EndUser)
        // var identity = new ClaimsIdentity(claims, "apiauth_type");
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        
        
        HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity),
            new AuthenticationProperties
            {
                IsPersistent = user.RememberMe
            }).Wait();

        return Ok(loggingCompanyUser);


    }

    
    [HttpPost]
    [Route("/claims")]
    public async Task<ActionResult<ClaimsIdentity>> ReturnUserClaims([FromBody] CompanyUser companyUser)
    {
        string isVerified = "";
        if (companyUser.VerifiedUser)
        {
            isVerified = "isVerified";
        }
        

        var claims = new List<Claim>();

        claims.Add(new Claim(ClaimTypes.Role, companyUser.Role));
        claims.Add(new Claim("isVerified", isVerified));
        claims.Add(new Claim(ClaimTypes.Email, companyUser.Email));
        //Add a claim to check for the type of the subclass(EndUser)
        // var identity = new ClaimsIdentity(claims, "apiauth_type");
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);


        HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity),
            new AuthenticationProperties
            {
                IsPersistent = companyUser.RememberMe
            }).Wait();


        return Ok(identity);        
    }

    // [Authorize]
    [Route("/cookie")]
    [HttpGet]
    public IActionResult GetCookie()
    {
        Console.WriteLine("in getting claims");
        if (HttpContext.User.IsInRole("Admin"))
        {
            Console.WriteLine("mf is Admin");
            return new ObjectResult("Admin");
        }
        else
        {
            Console.WriteLine("mf is User");

            return new ObjectResult("User");
        }
    }


    [HttpPost]
    [Route("/logout")]
    public async Task<ActionResult> LogOut()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return new ObjectResult("Success");
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


    
    // [Authorize]
    // [Route("/cookies")]
    // [HttpGet]
    // public 
    //
    
    

    [HttpPost]
    [Route("/picture")]
    public async Task<ActionResult> ReceivePictureFromUser([FromBody] PictureEmail pictureEmail)
    {

        CompanyUser companyUser = await context.CompanyUsers.FirstAsync(u => u.UserId == pictureEmail.userId);
        var companyId = context.CompanyUsers.FromSqlRaw("select MyCompanyCompanyId from Users where UserId = {0}", companyUser.UserId);
        CustomerCompany usersCompany = await context.CustomerCompanies.FirstAsync(c => c.CompanyId == companyUser.CompanyId);


        try
        {
            _userUpdater.UpdateUserNumberOfRequests(pictureEmail.userId);
            if (_emailSender.SendPictureFromUserToCompany(pictureEmail, companyUser, usersCompany.Email) == 1)
            {
                return Ok();
            }
            else if (_emailSender.SendPictureFromUserToCompany(pictureEmail, companyUser, usersCompany.Email) == 2)
            {
                return StatusCode(500);
            }
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
            throw;
        }

        return StatusCode(500);
    }
    
    
    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }
    
    
    private string CreateToken(CompanyUser companyUser)
    {
        List<Claim> claims = new List<Claim>
        {
            // new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, companyUser.Role)
        };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
            _configuration.GetSection("AppSettings:Token").Value));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(30),
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
    
    private RefreshToken GenerateRefreshToken()
    {
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expires = DateTime.Now.AddDays(7),
            Created = DateTime.Now
        };

        return refreshToken;
    }
    private async Task SetRefreshToken(RefreshToken newRefreshToken, CompanyUser companyUser, bool rememberMe)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = newRefreshToken.Expires
        };
     
        Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

        companyUser.RefreshToken = newRefreshToken.Token;
        companyUser.TokenCreated = newRefreshToken.Created;
        companyUser.TokenExpires = newRefreshToken.Expires;
        companyUser.RememberMe = rememberMe;
        

        context.CompanyUsers.Update(companyUser);
        await context.SaveChangesAsync();
    }
    
    
    
    
}


