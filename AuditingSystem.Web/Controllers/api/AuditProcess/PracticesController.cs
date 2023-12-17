using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuditingSystem.Web.Controllers.api.AuditProcess
{
    [Route("api/[controller]")]
    [ApiController]
    public class PracticesController : ControllerBase
    {
        private readonly IBaseRepository<Practice> _practiceRepository;
        public PracticesController(IBaseRepository<Practice> practiceRepository)
        {
            this._practiceRepository = practiceRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var tasks = _practiceRepository.ListAsync();

            return Ok(tasks);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _practiceRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"'{ex}'\nAn error occurred while deleting the user." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(Practice practice)
        {
            if (ModelState.IsValid)
            {
                await _practiceRepository.CreateAsync(practice);
                return NoContent();
            }


            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Practice practice)
        {
            if (ModelState.IsValid)
            {
                var prac = await _practiceRepository.FindByAsync(id);
                if (prac == null)
                {
                    return NotFound();
                }

                prac.Name = practice.Name;
                prac.Description = practice.Description;
                prac.TaskId = practice.TaskId;

                await _practiceRepository.UpdateAsync(practice);
                return NoContent();
            }

            return BadRequest();
        }
    }
}
