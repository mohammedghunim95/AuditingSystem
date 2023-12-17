using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuditingSystem.Web.Controllers.api.AuditProcess
{
    [Route("api/[controller]")]
    [ApiController]
    public class FunctionsController : ControllerBase
    {
        private readonly IBaseRepository<Function> _functionRepository;
        public FunctionsController(IBaseRepository<Function> functionRepository)
        {
            this._functionRepository = functionRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var functions = _functionRepository.ListAsync();

            return Ok(functions);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _functionRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"'{ex}'\nAn error occurred while deleting the user." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(Function function)
        {
            if (ModelState.IsValid)
            {
                await _functionRepository.CreateAsync(function);
                return NoContent();
            }


            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Function function)
        {
            if (ModelState.IsValid)
            {
                var func = await _functionRepository.FindByAsync(id);
                if (func == null)
                {
                    return NotFound();
                }

                func.Name = function.Name;
                func.Description = function.Description;
                func.DepartmentId = function.DepartmentId;

                await _functionRepository.UpdateAsync(func);
                return NoContent();
            }

            return BadRequest();
        }
    }
}
