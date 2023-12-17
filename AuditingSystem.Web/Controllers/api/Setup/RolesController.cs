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
    public class RolesController : ControllerBase
    {
        private readonly IBaseRepository<Role> _roleRepository; 
        public RolesController(IBaseRepository<Role> roleRepository)
        {
            _roleRepository = roleRepository; 
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _roleRepository.ListAsync();
            return Ok(list);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var role = _roleRepository.FindByAsync(id);
                if (role == null)
                {
                    return NotFound();
                }

                await _roleRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"'{ex}'\nAn error occurred while deleting the role." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(Role role)
        {
            if (ModelState.IsValid)
            {
                await _roleRepository.CreateAsync(role);
                return NoContent();
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, Role role)
        {
            if (ModelState.IsValid)
            {
                var rol = await _roleRepository.FindByAsync(id);
                if (rol == null)
                {
                    return NotFound();
                }

                rol.Name = role.Name;
                rol.Description = role.Description;
                await _roleRepository.UpdateAsync(rol);

                return NoContent();
            }

            return BadRequest();
        }

    }
}
