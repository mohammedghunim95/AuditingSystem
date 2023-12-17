using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AuditingSystem.Web.Controllers.Setup
{
    public class YearController : Controller
    {
        private readonly IBaseRepository<Year> _yearRepository;
        private readonly IBaseRepository<Company> _companyRepository;
        private readonly IBaseRepository<Department> _departmentRepository;
        public YearController(
            IBaseRepository<Year> yearRepository,
            IBaseRepository<Company> companyRepository,
            IBaseRepository<Department> departmentRepository)
        {
            _companyRepository = companyRepository;
            _departmentRepository = departmentRepository;
            _yearRepository = yearRepository;
        }
        public async Task<IActionResult> Index()
        {
            var year = await _yearRepository.ListAsync(
                  u => u.IsDeleted == false,
                  q => q.OrderBy(u => u.Name),
                  u => u.Company, u => u.Department);

            return View(year);
        }

        public async Task<IActionResult> Add()
        {
            var companies = await _companyRepository.ListAsync();
            var departments = await _departmentRepository.ListAsync();

            ViewBag.CompanyId = new SelectList(companies, "Id", "Name");
            ViewBag.DepartmentId = new SelectList(departments, "Id", "Name");

            return View();
        }


        public async Task<IActionResult> Edit(int id)
        {
            var year = await _yearRepository.FindByAsync(id);

            var companies = await _companyRepository.ListAsync();
            var departments = await _departmentRepository.ListAsync();

            ViewBag.CompanyId = new SelectList(companies, "Id", "Name", year.CompanyId);
            ViewBag.DepartmentId = new SelectList(departments, "Id", "Name", year.DepartmentId);

            return View(year);
        }

    }
}
