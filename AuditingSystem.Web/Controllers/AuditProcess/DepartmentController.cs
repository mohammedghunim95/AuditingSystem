using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AuditingSystem.Web.Controllers.AuditProcess
{
    public class DepartmentController : Controller
    {
        private readonly IBaseRepository<Department> _departmentRepository;
        private readonly IBaseRepository<Company> _companyRepository;
        public DepartmentController(
            IBaseRepository<Company> companyRepository,
            IBaseRepository<Department> departmentRepository)
        {
            _companyRepository = companyRepository;
            _departmentRepository = departmentRepository;
        }
        public async Task<IActionResult> Index()
        {
            var departments = await _departmentRepository.ListAsync(
                  c => c.IsDeleted == false,
                  q => q.OrderBy(u => u.Name),
                  c => c.Company);

            return View(departments);
        }

        public async Task<IActionResult> Add()
        {
            var company = _companyRepository.ListAsync(
                  u => u.IsDeleted == false,
                  q => q.OrderBy(u => u.Name),
                  null).Result;

            ViewBag.CompanyId = new SelectList(company, "Id", "Name");

            return View();
        }


        public async Task<IActionResult> Edit(int id)
        {
            var department = await _departmentRepository.FindByAsync(id);

            var company = _companyRepository.ListAsync(
                  u => u.IsDeleted == false,
                  q => q.OrderBy(u => u.Name),
                  null).Result;

            ViewBag.CompanyId = new SelectList(company, "Id", "Name", department.CompanyId);

            return View(department);
        }
    }
}
