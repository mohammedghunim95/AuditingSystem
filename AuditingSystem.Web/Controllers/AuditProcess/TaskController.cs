using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AuditingSystem.Web.Controllers.AuditProcess
{
    public class TaskController : Controller
    {
        private readonly IBaseRepository<Tasks> _tasksRepository;
        private readonly IBaseRepository<Objective> _objectiveRepository;
        public TaskController(
            IBaseRepository<Tasks> tasksRepository,
            IBaseRepository<Objective> objectiveRepository)
        {
            _tasksRepository = tasksRepository;
            _objectiveRepository = objectiveRepository;
        }

        public async Task<IActionResult> Index()
        {
            var tasks = await _tasksRepository.ListAsync(
                  c => c.IsDeleted == false,
                  q => q.OrderBy(u => u.Name),
                  c => c.Objective);

            return View(tasks);
        }

        public async Task<IActionResult> Add()
        {
            var objective = _objectiveRepository.ListAsync(
                  u => u.IsDeleted == false,
                  q => q.OrderBy(u => u.Name),
                  null).Result;

            ViewBag.ObjectiveId = new SelectList(objective, "Id", "Name");

            return View();
        }


        public async Task<IActionResult> Edit(int id)
        {
            var task = await _tasksRepository.FindByAsync(id);

            var objective = _objectiveRepository.ListAsync(
                  u => u.IsDeleted == false,
                  q => q.OrderBy(u => u.Name),
                  null).Result;

            ViewBag.ObjectiveId = new SelectList(objective, "Id", "Name", task.ObjectiveId);

            return View(task);
        }
    }
}
