using System.Security.Cryptography;
using EDHWebApp.Model;
using EDHWebApp.Persistance;
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

        [Route("/companies/")]
        [HttpGet]
        public async Task<IList<Company>> GetAllCompanies()
        {
            try
            {
                IList<Company> companies = await _edhContext.Companies.ToListAsync();
                return companies;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        [Route("/new/company")]
        [HttpPost]
        public async Task<ActionResult<Company>> AddCompanyAsync([FromBody] Company company)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
                
            }
            
            try
            {
               
                Company existentcompany = _edhContext.Companies.Where(u => u.Email.Equals(company.Email)).FirstOrDefault();
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
                await _edhContext.Companies.AddAsync(company);
                await _edhContext.SaveChangesAsync();
                return Created($"/{company.CompanyId}", company);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }

        }

        [Route("/company/removal/{CompanyId:int}")]
        [HttpDelete]
        public async Task RemoveCompanyAsync([FromRoute] int CompanyId)
        {

            try
            {
                Company company = _edhContext.Companies.FirstOrDefault(c => c.CompanyId == CompanyId);
                _edhContext.Companies.Remove(company);
                 await _edhContext.SaveChangesAsync();
                 
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }

    }