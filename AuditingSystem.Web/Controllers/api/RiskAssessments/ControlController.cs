using AuditingSystem.Database;
using AuditingSystem.Entities.RiskAssessments;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuditingSystem.Web.Controllers.api.RiskAssessment
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControlController : ControllerBase
    {
        private readonly IBaseRepository<Control> _controlRepository;
        private readonly IBaseRepository<RiskAssessmentList> _riskAssessmentListRepository;
        private readonly IBaseRepository<RiskIdentification> _riskIdentificationRepository;
        private readonly AuditingSystemDbContext db;
        public ControlController(IBaseRepository<Control> controlRepository, IBaseRepository<RiskAssessmentList> riskAssessmentListRepository,AuditingSystemDbContext db, IBaseRepository<RiskIdentification> riskIdentificationRepository)
        {
            this._controlRepository = controlRepository;
            _riskAssessmentListRepository = riskAssessmentListRepository;
            this.db = db;
            _riskIdentificationRepository = riskIdentificationRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var control = _controlRepository.ListAsync();

            return Ok(control);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _controlRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"'{ex}'\nAn error occurred while deleting the user." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(Control control)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _controlRepository.CreateAsync(control);

                    var con = await _controlRepository.FindByAsync(control.Id);
                    var inherent = await _riskIdentificationRepository.FindByAsync(control.RiskIdentificationId);

                    var controlRate = con.ControlEffectivenessRating;
                    var inherentRate = inherent.InherentRiskRating;




                    RiskAssessmentList riskAssessment = new RiskAssessmentList();
                    riskAssessment.ControlId = con.Id;
                    riskAssessment.RiskIdentificationId = con.RiskIdentificationId;                    
                    riskAssessment.IsDeleted = false;
                    riskAssessment.CreatedBy = "admin";
                    riskAssessment.UpdatedBy = "admin";
                    riskAssessment.CreationDate = DateTime.Now;
                    riskAssessment.UpdatedDate = DateTime.Now;
                    riskAssessment.Description = "test";
                    riskAssessment.Name = "test";

                    if (controlRate >= 4 && inherentRate >= 6)
                    {
                        riskAssessment.ResidualRiskRating = "Active Management";
                    }
                    else if (controlRate <= 4 && inherentRate >= 6)
                    {
                        riskAssessment.ResidualRiskRating = "Continuous Review";
                    }
                    else if (controlRate >= 4 && inherentRate <= 6)
                    {
                        riskAssessment.ResidualRiskRating = "Periodic Monitoring";
                    }
                    else if (controlRate <= 4 && inherentRate <= 6)
                    {
                        riskAssessment.ResidualRiskRating = "No major concern";
                    }

                    db.RiskAssessmentsList.Add(riskAssessment);
                    db.SaveChanges();

                    return NoContent();
                }
            }
            catch (Exception ex)
            { 
                Console.WriteLine(ex); 
                throw;
            }

            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] Control control)
        {
            if (ModelState.IsValid)
            {
                var cntr = await _controlRepository.FindByAsync(id);
                if (cntr == null)
                {
                    return NotFound();
                }

                cntr.Name = control.Name;
                cntr.Description = control.Description;
                cntr.ControlTypeId = control.ControlTypeId;
                cntr.ControlProcessId = control.ControlProcessId;
                cntr.ControlFrequencyId = control.ControlFrequencyId;
                cntr.ControlEffectivenessId = control.ControlEffectivenessId;
                cntr.ControlEffectivenessRating = control.ControlEffectivenessRating;
                cntr.RiskIdentificationId = control.RiskIdentificationId;

                await _controlRepository.UpdateAsync(cntr);
                return NoContent();
            }

            return BadRequest();
        }
    }
}
