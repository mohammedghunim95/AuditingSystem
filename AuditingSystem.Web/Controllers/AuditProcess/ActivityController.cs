using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AuditingSystem.Web.Controllers.AuditProcess
{
    public class ActivityController : Controller
    {
        private readonly IBaseRepository<Activity> _activityRepository;
        private readonly IBaseRepository<Function> _functionRepository;
        public ActivityController(
            IBaseRepository<Activity> activityRepository,
            IBaseRepository<Function> functionRepository)
        {
            _activityRepository = activityRepository;
            _functionRepository = functionRepository;
        }
        public async Task<IActionResult> Index()
        {
            var activities = await _activityRepository.ListAsync(
                  c => c.IsDeleted == false,
                  q => q.OrderBy(u => u.Name),
                  c => c.Function);

            return View(activities);
        }

        public async Task<IActionResult> Add()
        {
            var function = _functionRepository.ListAsync(
                  u => u.IsDeleted == false,
                  q => q.OrderBy(u => u.Name),
                  null).Result;

            ViewBag.FunctionId = new SelectList(function, "Id", "Name");

            return View();
        }


        public async Task<IActionResult> Edit(int id)
        {
            var activity = await _activityRepository.FindByAsync(id);

            var function = _functionRepository.ListAsync(
                  u => u.IsDeleted == false,
                  q => q.OrderBy(u => u.Name),
                  null).Result;

            ViewBag.FunctionId = new SelectList(function, "Id", "Name", activity.FunctionId);

            return View(activity);
        }
    }
}
