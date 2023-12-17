using AuditingSystem.Database;
using AuditingSystem.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using AuditingSystem.Services.Interfaces;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Entities.AuditProcess;

namespace AuditingSystem.Web.Controllers.api.AuditProcess
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IBaseRepository<Company> _companyRepository;
        public CompaniesController(IBaseRepository<Company> companyRepository)
        {
            this._companyRepository = companyRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var companies = _companyRepository.ListAsync();

            return Ok(companies);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _companyRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"'{ex}'\nAn error occurred while deleting the user." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(Company company)
        {
            if (ModelState.IsValid)
            {
                await _companyRepository.CreateAsync(company);
                return NoContent();
            }
             

            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Company company)
        {
            if (ModelState.IsValid)
            {
                var com = await _companyRepository.FindByAsync(id);
                if (com == null)
                {
                    return NotFound();
                }
                                 
                com.Name = company.Name;
                com.Description = company.Description;
                com.Address = company.Address;
                com.ContactNo = company.ContactNo;
                com.Email = company.Email;
                com.IndustryId = company.IndustryId;
                
                await _companyRepository.UpdateAsync(com);
                return NoContent();
            }

            return BadRequest();
        }




    }
}
