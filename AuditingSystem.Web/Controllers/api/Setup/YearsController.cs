using AuditingSystem.Database;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuditingSystem.Web.Controllers.api.Setup
{
    [Route("api/[controller]")]
    [ApiController]
    public class YearsController : ControllerBase
    {
        private readonly IBaseRepository<Year> _yearRepository; 
        public YearsController(IBaseRepository<Year> yearRepository)
        {
            this._yearRepository = yearRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users =await _yearRepository.ListAsync();

            return Ok(users);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _yearRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"'{ex}'\nAn error occurred while deleting the user." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(Year year)
        {
            if (ModelState.IsValid)
            {
                await _yearRepository.CreateAsync(year);
                return NoContent();
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, Year year)
        {
            if (ModelState.IsValid)
            {
                var years = await _yearRepository.FindByAsync(id);
                if (years == null)
                {
                    return NotFound();
                }

                years.Name = year.Name;
                years.CompanyId = year.CompanyId;
                years.DepartmentId = year.DepartmentId;
                years.Description = year.Description;

                await _yearRepository.UpdateAsync(years);
                return NoContent();
            }

            return BadRequest();
        }

    }
}
