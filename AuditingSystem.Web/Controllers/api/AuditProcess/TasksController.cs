using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuditingSystem.Web.Controllers.api.AuditProcess
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly IBaseRepository<Tasks> _taskRepository;
        public TasksController(IBaseRepository<Tasks> taskRepository)
        {
            this._taskRepository = taskRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var tasks = _taskRepository.ListAsync();

            return Ok(tasks);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _taskRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"'{ex}'\nAn error occurred while deleting the user." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(Tasks tasks)
        {
            if (ModelState.IsValid)
            {
                await _taskRepository.CreateAsync(tasks);
                return NoContent();
            }


            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Tasks tasks)
        {
            if (ModelState.IsValid)
            {
                var task = await _taskRepository.FindByAsync(id);
                if (task == null)
                {
                    return NotFound();
                }

                task.Name = tasks.Name;
                task.Description = tasks.Description;
                task.ObjectiveId = tasks.ObjectiveId;

                await _taskRepository.UpdateAsync(task);
                return NoContent();
            }

            return BadRequest();
        }
    }
}
