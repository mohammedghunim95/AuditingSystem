using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuditingSystem.Web.Controllers.api.AuditProcess
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndustriesController : ControllerBase
    {
        private readonly IBaseRepository<Industry> _industryRepository;
        public IndustriesController(IBaseRepository<Industry> industryRepository)
        {
            this._industryRepository = industryRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _industryRepository.ListAsync();

            return Ok(users);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _industryRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"'{ex}'\nAn error occurred while deleting the user." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(Industry industry)
        {
            if (ModelState.IsValid)
            {
                await _industryRepository.CreateAsync(industry);
                return NoContent();
            }


            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Industry industry)
        {
            if (ModelState.IsValid)
            {
                var ind = await _industryRepository.FindByAsync(id);
                if (ind == null)
                {
                    return NotFound();
                }

                ind.Name = industry.Name;
                ind.Description = industry.Description;
                ind.ParentIndustryId = industry.ParentIndustryId;
                

                await _industryRepository.UpdateAsync(ind);
                return NoContent();
            }

            return BadRequest();
        }


    }
}
