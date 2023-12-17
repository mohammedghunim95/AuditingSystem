using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.Setup;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AuditingSystem.Web.Controllers.AuditProcess
{
    public class CompanyController : Controller
    {
        private readonly IBaseRepository<Company> _companyRepository;
        private readonly IBaseRepository<Industry> _industryRepository;
        public CompanyController(
            IBaseRepository<Company> companyRepository,
            IBaseRepository<Industry> industryRepository)
        {
            _companyRepository = companyRepository;
            _industryRepository = industryRepository;
        }
        public async Task<IActionResult> Index()
        {
            var companies = await _companyRepository.ListAsync(
                  c => c.IsDeleted == false,
                  q => q.OrderBy(u => u.Name),
                  c => c.Industry);

            return View(companies);
        }

        public async Task<IActionResult> Add()
        {
            var industry = _industryRepository.ListAsync(
                  u => u.IsDeleted == false,
                  q => q.OrderBy(u => u.Name),
                  null).Result;

            ViewBag.IndustryId = new SelectList(industry, "Id", "Name");

            return View();
        }


        public async Task<IActionResult> Edit(int id)
        {
            var company = await _companyRepository.FindByAsync(id);

            var industry = _industryRepository.ListAsync(
                  u => u.IsDeleted == false,
                  q => q.OrderBy(u => u.Name),
                  null).Result;

            ViewBag.IndustryId = new SelectList(industry, "Id", "Name", company.IndustryId);

            return View(company);
        }


    }
}
