using AuditingSystem.Database;
using AuditingSystem.Entities.Lockups;
using AuditingSystem.Entities.RiskAssessments;
using AuditingSystem.Services.Interfaces;
using AuditingSystem.Services.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AuditingSystem.Web.Controllers.RiskAssessments
{
    public class RiskAssessmentController : Controller
    {
            private readonly IBaseRepository<RiskAssessmentList> _riskAssessmentListRepository;
        private readonly AuditingSystemDbContext db;
            public RiskAssessmentController(
                IBaseRepository<RiskAssessmentList> riskAssessmentList, AuditingSystemDbContext db)
            {
                _riskAssessmentListRepository = riskAssessmentList;
                this.db = db;
            }
            public async Task<IActionResult> Index()
            {
            //var controlRepository = await _riskAssessmentListRepository.ListAsync(
            //      c => c.IsDeleted == false,
            //      q => q.OrderBy(u => u.Name),
            //      c => c.RiskIdentification, c => c.Control);

            var list = from riskAssessment in db.RiskAssessmentsList
                       join riskIdentification in db.RiskIdentifications on riskAssessment.RiskIdentificationId equals riskIdentification.Id
                       join control in db.Controls on riskAssessment.ControlId equals control.Id
                       join riskCategory in db.RiskCategories on riskIdentification.RiskCategoryId equals riskCategory.Id
                       join riskImpact in db.RiskImpacts on riskIdentification.RiskImpactId equals riskImpact.Id
                       join riskLikelihood in db.RiskLikehoods on riskIdentification.RiskLikelihoodId equals riskLikelihood.Id
                       join controlType in db.ControlTypes on control.ControlTypeId equals controlType.Id
                       join controlProcess in db.ControlProcesses on control.ControlProcessId equals controlProcess.Id
                       join controlFrequency in db.ControlFrequencies on control.ControlFrequencyId equals controlFrequency.Id
                       join controlEffectiveness in db.ControlEffectivenesses on control.ControlEffectivenessId equals controlEffectiveness.Id
                       select new RiskAssessmentVM
                       {
                           RiskAssessmentList = riskAssessment,
                           RiskIdentification = riskIdentification,
                           RiskCategory = riskCategory,
                           RiskImpact = riskImpact,
                           RiskLikehood = riskLikelihood,
                           Control = control,
                           ControlType = controlType,
                           ControlProcess = controlProcess,
                           ControlFrequency = controlFrequency,
                           ControlEffectiveness = controlEffectiveness
                       };

            return View(list.ToList());


            return View(list.ToList());
             
            }
        
    }
}
