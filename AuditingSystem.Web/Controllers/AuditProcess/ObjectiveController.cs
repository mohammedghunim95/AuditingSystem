using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AuditingSystem.Web.Controllers.AuditProcess
{
    public class ObjectiveController : Controller
    {
        private readonly IBaseRepository<Activity> _activityRepository;
        private readonly IBaseRepository<Objective> _objectiveRepository;
        public ObjectiveController(
            IBaseRepository<Activity> activityRepository,
            IBaseRepository<Objective> objectiveRepository)
        {
            _activityRepository = activityRepository;
            _objectiveRepository = objectiveRepository;
        }
        public async Task<IActionResult> Index()
        {
            var objectives = await _objectiveRepository.ListAsync(
                  c => c.IsDeleted == false,
                  q => q.OrderBy(u => u.Name),
                  c => c.Activity);

            return View(objectives);
        }

        public async Task<IActionResult> Add()
        {
            var activity = _activityRepository.ListAsync(
                  u => u.IsDeleted == false,
                  q => q.OrderBy(u => u.Name),
                  null).Result;

            ViewBag.ActivityId = new SelectList(activity, "Id", "Name");

            return View();
        }


        public async Task<IActionResult> Edit(int id)
        {
            var objective = await _objectiveRepository.FindByAsync(id);

            var activity = _activityRepository.ListAsync(
                  u => u.IsDeleted == false,
                  q => q.OrderBy(u => u.Name),
                  null).Result;

            ViewBag.ActivityId = new SelectList(activity, "Id", "Name", objective.ActivityId);

            return View(objective);
        }
    }
}
