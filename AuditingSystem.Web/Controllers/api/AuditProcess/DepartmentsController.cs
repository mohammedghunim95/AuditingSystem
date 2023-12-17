using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuditingSystem.Web.Controllers.api.AuditProcess
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IBaseRepository<Department> _departmentRepository;
        public DepartmentsController(IBaseRepository<Department> departmentRepository)
        {
            this._departmentRepository = departmentRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var departments = _departmentRepository.ListAsync();

            return Ok(departments);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _departmentRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"'{ex}'\nAn error occurred while deleting the user." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(Department department)
        {
            if (ModelState.IsValid)
            {
                await _departmentRepository.CreateAsync(department);
                return NoContent();
            }


            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Department department)
        {
            if (ModelState.IsValid)
            {
                var dept = await _departmentRepository.FindByAsync(id);
                if (dept == null)
                {
                    return NotFound();
                }

                dept.Name = department.Name;
                dept.Description = department.Description;
                dept.CompanyId = department.CompanyId;

                await _departmentRepository.UpdateAsync(dept);
                return NoContent();
            }

            return BadRequest();
        }
    }
}
