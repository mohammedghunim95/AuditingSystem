using AuditingSystem.Database;
using AuditingSystem.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using AuditingSystem.Services.Interfaces;
using AuditingSystem.Entities.Setup;

namespace AuditingSystem.Web.Controllers.api.Setup
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IBaseRepository<User> _userRepository;
        public UsersController(IBaseRepository<User> userRepository)
        {
            this._userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userRepository.ListAsync();

            return Ok(users);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _userRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"'{ex}'\nAn error occurred while deleting the user." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(User user)
        {
            if (ModelState.IsValid)
            {
                await _userRepository.CreateAsync(user);
                return NoContent();
            }
             

            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                var usr = await _userRepository.FindByAsync(id);
                if (usr == null)
                {
                    return NotFound();
                }

                // تحديث الخصائص
                usr.Title = user.Title;
                usr.Email = user.Email;
                usr.Name = user.Name;
                usr.CompanyId = user.CompanyId;
                usr.DepartmentId = user.DepartmentId;
                usr.RoleId = user.RoleId;
                usr.ContactNo = user.ContactNo;
                usr.Description = user.Description;

                await _userRepository.UpdateAsync(usr);
                return NoContent();
            }

            return BadRequest();
        }




    }
}
