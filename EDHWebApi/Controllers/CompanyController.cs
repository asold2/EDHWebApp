using System.Security.Cryptography;
using EDHWebApi.Model;
using EDHWebApi.Persistance;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EDHWebApi.Controllers;

[ApiController]
[Route("[controller]")]
    public class CompanyController : Controller
    {
        private EDHContext _edhContext;

        public CompanyController(EDHContext edhContext)
        {
            this._edhContext = edhContext;
        }

        //Returns list of all companies
        [Route("/companies/")]
        [HttpGet]
        public async Task<List<CustomerCompany>> GetAllCompanies()
        {
            try
            {
                List<CustomerCompany> companies = await _edhContext.CustomerCompanies.ToListAsync();
            return companies;

        }
        catch (Exception e)
            {
                return null;
            }
        }
        //Add new company to database
        [Route("/new/company")]
        [HttpPost]
        public async Task<ActionResult<CustomerCompany>> AddCompanyAsync([FromBody] CustomerCompany company)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
                
            }
            
            try
            {
               
                CustomerCompany existentcompany = _edhContext.CustomerCompanies.Where(u => u.Email.Equals(company.Email)).FirstOrDefault();
                if (existentcompany != null)
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
                company.CreationDate = DateTime.Now;
                await _edhContext.CustomerCompanies.AddAsync(company);
                await _edhContext.SaveChangesAsync();
                return Created($"/{company.CompanyId}", company);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }

        }


      


        //Remove company by it's Id
            // The users under the company are not currently removed
        [Route("/company/{CompanyId:int}")]
        [HttpDelete]
        public async Task RemoveCompanyAsync([FromRoute] int CompanyId)
        {

            try
            {
                CustomerCompany company = _edhContext.CustomerCompanies.FirstOrDefault(c => c.CompanyId == CompanyId);
                _edhContext.CustomerCompanies.Remove(company);
                foreach (CompanyUser user in _edhContext.CompanyUsers.Where(u=>u.CompanyId==company.CompanyId))
                {
                    _edhContext.Remove(user);
                }
                
                
                 await _edhContext.SaveChangesAsync();
                 
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }

    }