using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AuditingSystem.Web.Controllers.Setup
{
    public class UserController : Controller
    {
        private readonly IBaseRepository<Company> _companyRepository;
        private readonly IBaseRepository<Department> _departmentRepository;
        private readonly IBaseRepository<Role> _rolesRepository;
        private readonly IBaseRepository<User> _userRepository;
        public UserController(
            IBaseRepository<Department> departmentRepository,
            IBaseRepository<Role> roleRepository,
            IBaseRepository<Company> companyRepository,
            IBaseRepository<User> userRepository)
        {
            _companyRepository = companyRepository;
            _departmentRepository = departmentRepository;
            _rolesRepository = roleRepository;
            _userRepository = userRepository;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.ListAsync(
                  u => u.IsDeleted == false,
                  q => q.OrderBy(u => u.Name),
                  u => u.Company, u => u.Department, u => u.Role);

            return View(users);
        }

        public async Task<IActionResult> Add()
        {
            var companies = _companyRepository.ListAsync(
                  u => u.IsDeleted == false,
                  q => q.OrderBy(u => u.Name),
                  null).Result;
            var departments = _departmentRepository.ListAsync(
                  u => u.IsDeleted == false,
                  q => q.OrderBy(u => u.Name),
                  null).Result;
            var roles = _rolesRepository.ListAsync(
                  u => u.IsDeleted == false,
                  q => q.OrderBy(u => u.Name),
                  null).Result;

            ViewBag.CompanyId = new SelectList(companies, "Id", "Name");
            ViewBag.DepartmentId = new SelectList(departments, "Id", "Name");
            ViewBag.RoleId = new SelectList(roles, "Id", "Name");

            return View();
        }


        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userRepository.FindByAsync(id);

            var companies = _companyRepository.ListAsync(
                  u => u.IsDeleted == false,
                  q => q.OrderBy(u => u.Name),
                  null).Result;
            var departments = _departmentRepository.ListAsync(
                  u => u.IsDeleted == false,
                  q => q.OrderBy(u => u.Name),
                  null).Result;
            var roles = _rolesRepository.ListAsync(
                  u => u.IsDeleted == false,
                  q => q.OrderBy(u => u.Name),
                  null).Result;

            ViewBag.CompanyId = new SelectList(companies, "Id", "Name", user.CompanyId);
            ViewBag.DepartmentId = new SelectList(departments, "Id", "Name", user.DepartmentId);
            ViewBag.RoleId = new SelectList(roles, "Id", "Name", user.RoleId);

            return View(user);
        }


    }
}
