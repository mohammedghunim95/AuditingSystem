using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.Lockups;
using AuditingSystem.Entities.RiskAssessments;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AuditingSystem.Web.Controllers.RiskAssessments
{
    public class ControlController : Controller
    {
        private readonly IBaseRepository<Control> _controlRepository;
        private readonly IBaseRepository<ControlType> _controlTypeRepository;
        private readonly IBaseRepository<ControlProcess> _controlProcessRepository;
        private readonly IBaseRepository<ControlFrequency> _controlFrequencyRepository;
        private readonly IBaseRepository<ControlEffectiveness> _controlEffectivenessRepository;
        private readonly IBaseRepository<RiskIdentification> _riskIdentificationRepository;
        public ControlController(
            IBaseRepository<Control> controlRepository,
            IBaseRepository<ControlEffectiveness> controlEffectivenessRepository,
            IBaseRepository<ControlFrequency> controlFrequencyRepository,
            IBaseRepository<ControlProcess> controlProcessRepository,
            IBaseRepository<ControlType> controlTypeRepository,
            IBaseRepository<RiskIdentification> riskIdentificationRepository)
        {
            _controlRepository = controlRepository;
            _controlEffectivenessRepository = controlEffectivenessRepository;
            _controlFrequencyRepository = controlFrequencyRepository;
            _controlProcessRepository = controlProcessRepository;
            _controlTypeRepository = controlTypeRepository;
            _riskIdentificationRepository = riskIdentificationRepository;
        }
        public async Task<IActionResult> Index()
        {
            var controlRepository = await _controlRepository.ListAsync(
                  c => c.IsDeleted == false,
                  q => q.OrderBy(u => u.Name),
                  c => c.ControlType, c=> c.ControlProcess, c=> c.ControlFrequency, c=>c.ControlEffectiveness, c=>c.RiskIdentification);

            return View(controlRepository);
        }

        public async Task<IActionResult> Add(int? riskIdentificationId)
        {
            var controlType = await _controlTypeRepository.ListAsync(
                u => u.IsDeleted == false,
                q => q.OrderBy(u => u.Name),
                null);

            var controlProcess = await _controlProcessRepository.ListAsync(
                u => u.IsDeleted == false,
                q => q.OrderBy(u => u.Name),
                null);

            var controlFrequency = await _controlFrequencyRepository.ListAsync(
                u => u.IsDeleted == false,
                q => q.OrderBy(u => u.Name),
                null);

            var controlEffectiveness = await _controlEffectivenessRepository.ListAsync(
                u => u.IsDeleted == false,
                q => q.OrderBy(u => u.Id),
                null);

            var riskIdentification = await _riskIdentificationRepository.ListAsync(
                u => u.IsDeleted == false,
                q => q.OrderBy(u => u.Name),
                null);

            // Filter Risk Identification based on the provided ID
            if (riskIdentificationId.HasValue)
            {
                riskIdentification = riskIdentification.Where(r => r.Id == riskIdentificationId.Value);
            }

            ViewBag.ControlTypeId = new SelectList(controlType, "Id", "Name");
            ViewBag.ControlProcessId = new SelectList(controlProcess, "Id", "Name");
            ViewBag.ControlFrequencyId = new SelectList(controlFrequency, "Id", "Name");
            ViewBag.ControlEffectivenessId = new SelectList(controlEffectiveness.Select(e => new { Id = e.Id, Name = $"{e.Name} - {e.Rate}" }), "Id", "Name");
            ViewBag.RiskIdentificationId = new SelectList(riskIdentification, "Id", "Name");

            return View();
        }



        public async Task<IActionResult> Edit(int id)
        {
            var control = await _controlRepository.FindByAsync(id);

            var controlType = _controlTypeRepository.ListAsync(
                  u => u.IsDeleted == false,
                  q => q.OrderBy(u => u.Name),
                  null).Result;

            var controlProcess = _controlProcessRepository.ListAsync(
                  u => u.IsDeleted == false,
                  q => q.OrderBy(u => u.Name),
                  null).Result;

            var controlFrequency = _controlFrequencyRepository.ListAsync(
                  u => u.IsDeleted == false,
                  q => q.OrderBy(u => u.Name),
                  null).Result;

            var controlEffectiveness = _controlEffectivenessRepository.ListAsync(
                  u => u.IsDeleted == false,
                  q => q.OrderBy(u => u.Name),
                  null).Result;

            ViewBag.ControlTypeId = new SelectList(controlType, "Id", "Name", control.ControlTypeId);
            ViewBag.ControlProcessId = new SelectList(controlProcess, "Id", "Name", control.ControlProcess);
            ViewBag.ControlFrequencyId = new SelectList(controlFrequency, "Id", "Name", control.ControlFrequencyId);
            ViewBag.ControlEffectivenessId = new SelectList(controlEffectiveness, "Id", "Name", control.ControlEffectivenessId);

            return View(control);
        }
    }
}
