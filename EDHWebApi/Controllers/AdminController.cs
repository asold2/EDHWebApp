using System.Security.Cryptography;
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
    public async Task<ActionResult<CompanyUser>> AddUserAsync([FromBody] CompanyUser companyUser)
    {
       
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            CompanyUser existentCompanyUser = await context.CompanyUsers.FirstOrDefaultAsync(u => u.Email.Equals(companyUser.Email));
            if (existentCompanyUser != null)
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
            CustomerCompany company = await context.CustomerCompanies.FirstAsync(c => c.CompanyId == companyUser.MyCompany.CompanyId);
            companyUser.MyCompany = company;
            companyUser.CompanyId = company.CompanyId;
            await context.CompanyUsers.AddAsync(companyUser);
            await context.SaveChangesAsync();
            return Created($"/{companyUser.UserId}", companyUser);
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
        CompanyUser companyUserToDelete = context.CompanyUsers.FirstOrDefault(u => u.UserId == userId);
        try
        {
            context.CompanyUsers.Remove(companyUserToDelete);
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw e;
        }

    }
    
    
    [Route("/registration/")]
    [HttpPut]
    public async Task<ActionResult<CompanyUser>> RegisterUser([FromBody] Account account)
    {
        CompanyUser existentCompanyUserName = context.CompanyUsers.FirstOrDefault(u => u.Username.Equals(account.Username));

        if (existentCompanyUserName != null)
        {
            return StatusCode(403);
        }
        
        CompanyUser companyUser = await context.CompanyUsers.FirstAsync(u => u.UserId == account.userId);

        CreatePasswordHash(account.Password, out byte[] passwordHash, out byte[] passwordSalt);

        companyUser.Username = account.Username;
        companyUser.PasswordHash = passwordHash;
        companyUser.PasswordSalt = passwordSalt;
        companyUser.VerifiedUser = true;

        
        
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _userUpdater.RegisterUser(companyUser);

            return   Accepted($"/{companyUser.UserId}", companyUser);
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
    public async Task<ActionResult<CompanyUser>> AddRegisteredUser([FromBody] CompanyUser companyUser)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            

            
            context.CompanyUsers.Update(companyUser);
            await context.SaveChangesAsync();
            return Created($"/{companyUser.UserId}", companyUser);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }

    }
    
    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
    
    
   

}