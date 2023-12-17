using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuditingSystem.Web.Controllers.api.AuditProcess
{
    [Route("api/[controller]")]
    [ApiController]
    public class ObjectivesController : ControllerBase
    {
        private readonly IBaseRepository<Objective> _objectiveRepository;
        public ObjectivesController(IBaseRepository<Objective> abjectiveRepository)
        {
            this._objectiveRepository = abjectiveRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var objectives = _objectiveRepository.ListAsync();

            return Ok(objectives);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _objectiveRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"'{ex}'\nAn error occurred while deleting the user." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(Objective objective)
        {
            if (ModelState.IsValid)
            {
                await _objectiveRepository.CreateAsync(objective);
                return NoContent();
            }


            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Objective objective)
        {
            if (ModelState.IsValid)
            {
                var obj = await _objectiveRepository.FindByAsync(id);
                if (obj == null)
                {
                    return NotFound();
                }

                obj.Name = objective.Name;
                obj.Description = objective.Description;
                obj.ActivityId = objective.ActivityId;

                await _objectiveRepository.UpdateAsync(obj);
                return NoContent();
            }

            return BadRequest();
        }
    }
}
