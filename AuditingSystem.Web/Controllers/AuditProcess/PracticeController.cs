using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AuditingSystem.Web.Controllers.AuditProcess
{
    public class PracticeController : Controller
    {
        private readonly IBaseRepository<Tasks> _tasksRepository;
        private readonly IBaseRepository<Practice> _practiceRepository;
        public PracticeController(
            IBaseRepository<Tasks> tasksRepository,
            IBaseRepository<Practice> practiceRepository)
        {
            _tasksRepository = tasksRepository;
            _practiceRepository = practiceRepository;
        }

        public async Task<IActionResult> Index()
        {
            var tasks = await _practiceRepository.ListAsync(
                  c => c.IsDeleted == false,
                  q => q.OrderBy(u => u.Name),
                  c => c.Task);

            return View(tasks);
        }

        public async Task<IActionResult> Add()
        {
            var task = _tasksRepository.ListAsync(
                  u => u.IsDeleted == false,
                  q => q.OrderBy(u => u.Name),
                  null).Result;

            ViewBag.TaskId = new SelectList(task, "Id", "Name");

            return View();
        }


        public async Task<IActionResult> Edit(int id)
        {
            var practice = await _practiceRepository.FindByAsync(id);

            var task = _tasksRepository.ListAsync(
                  u => u.IsDeleted == false,
                  q => q.OrderBy(u => u.Name),
                  null).Result;

            ViewBag.TaskId = new SelectList(task, "Id", "Name", practice.TaskId);

            return View(practice);
        }
    }
}
