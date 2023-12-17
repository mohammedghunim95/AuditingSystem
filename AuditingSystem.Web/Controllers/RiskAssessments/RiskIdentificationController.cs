using AuditingSystem.Entities.Lockups;
using AuditingSystem.Entities.RiskAssessments;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AuditingSystem.Web.Controllers.RiskAssessments
{
    public class RiskIdentificationController : Controller
    {
        private readonly IBaseRepository<RiskIdentification> _riskIdentificaionRepository;

        private readonly IBaseRepository<RiskCategory> _riskCategoryRepository;
        private readonly IBaseRepository<RiskImpact> _riskImpactRepository;
        private readonly IBaseRepository<RiskLikehood> _likelihoodRepository;

        public RiskIdentificationController(
            IBaseRepository<RiskIdentification> riskIdentificaionRepository,
            IBaseRepository<RiskCategory> riskCategoryRepository,
            IBaseRepository<RiskImpact> riskImpactRepository,
            IBaseRepository<RiskLikehood> likelihoodRepository)
        {
            _riskIdentificaionRepository = riskIdentificaionRepository;
            _riskCategoryRepository = riskCategoryRepository;
            _riskImpactRepository = riskImpactRepository;
            _likelihoodRepository = likelihoodRepository;

        }
        public async Task<IActionResult> Index()
        {
            var riskIdentifications = await _riskIdentificaionRepository.ListAsync(
                  c => c.IsDeleted == false,
                  q => q.OrderBy(u => u.Name),
                  c => c.RiskCategory, c => c.RiskImpact, c => c.RiskLikelihood);

            return View(riskIdentifications);
        }

        public async Task<IActionResult> Add()
        {
            var riskCategory = _riskCategoryRepository.ListAsync(
                  u => u.IsDeleted == false,
                  q => q.OrderBy(u => u.Name),
                  null).Result;
            var riskImpact = _riskImpactRepository.ListAsync(
                  u => u.IsDeleted == false,
                  q => q.OrderBy(u => u.Name),
                  null).Result;
            var likelihood = _likelihoodRepository.ListAsync(
                  u => u.IsDeleted == false,
                  q => q.OrderBy(u => u.Name),
                  null).Result;

            ViewBag.RiskCategoryId = new SelectList(riskCategory, "Id", "Name");
            ViewBag.RiskImpactId = new SelectList(riskImpact.Select(r => new { Id = r.Id, Name = $"{r.Name} - {r.Rate}" }), "Id", "Name");
            ViewBag.RiskLikelihoodId = new SelectList(likelihood.Select(l => new { Id = l.Id, Name = $"{l.Name} - {l.Rate}" }), "Id", "Name");



            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddWithLink(RiskIdentification riskIdentification)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _riskIdentificaionRepository.CreateAsync(riskIdentification);

                    // Assuming the repository returns the saved entity with its ID
                    var savedRiskIdentification = await _riskIdentificaionRepository.FindByAsync(riskIdentification.Id);

                    return Ok(new { id = savedRiskIdentification.Id });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal Server Error");
            }

            return BadRequest("Invalid ModelState");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var riskIdentification = await _riskIdentificaionRepository.FindByAsync(id);

            var riskCategory = _riskCategoryRepository.ListAsync(
                  u => u.IsDeleted == false,
                  q => q.OrderBy(u => u.Name),
                  null).Result;
            var riskImpact = _riskImpactRepository.ListAsync(
                  u => u.IsDeleted == false,
                  q => q.OrderBy(u => u.Name),
                  null).Result;
            var likelihood = _likelihoodRepository.ListAsync(
                  u => u.IsDeleted == false,
                  q => q.OrderBy(u => u.Name),
                  null).Result;

            ViewBag.RiskCategoryId = new SelectList(riskCategory, "Id", "Name", riskIdentification.RiskCategoryId);
            ViewBag.RiskImpactId = new SelectList(riskImpact, "Id", "Name", riskIdentification.RiskImpactId);
            ViewBag.RiskLikelihoodId = new SelectList(likelihood, "Id", "Name", riskIdentification.RiskLikelihoodId);

            return View(riskIdentification);
        }
    }
}
