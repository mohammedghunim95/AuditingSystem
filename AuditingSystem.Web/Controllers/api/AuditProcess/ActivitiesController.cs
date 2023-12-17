using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuditingSystem.Web.Controllers.api.AuditProcess
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly IBaseRepository<Activity> _activityRepository;
        public ActivitiesController(IBaseRepository<Activity> activityRepository)
        {
            this._activityRepository = activityRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var activity = _activityRepository.ListAsync();

            return Ok(activity);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _activityRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"'{ex}'\nAn error occurred while deleting the user." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(Activity activity)
        {
            if (ModelState.IsValid)
            {
                await _activityRepository.CreateAsync(activity);
                return NoContent();
            }


            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Activity activity)
        {
            if (ModelState.IsValid)
            {
                var act = await _activityRepository.FindByAsync(id);
                if (act == null)
                {
                    return NotFound();
                }

                act.Name = activity.Name;
                act.Description = activity.Description;
                act.FunctionId = activity.FunctionId;

                await _activityRepository.UpdateAsync(act);
                return NoContent();
            }

            return BadRequest();
        }
    }
}
