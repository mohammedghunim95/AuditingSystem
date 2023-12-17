using AuditingSystem.Entities.RiskAssessments;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuditingSystem.Web.Controllers.api.RiskAssessments
{
    [Route("api/[controller]")]
    [ApiController]
    public class RiskAssessmentsController : ControllerBase
    {
        private readonly IBaseRepository<RiskAssessmentList> _riskAssessmentListRepository;
        public RiskAssessmentsController(IBaseRepository<RiskAssessmentList> riskAssessmentListRepository)
        {
            this._riskAssessmentListRepository = riskAssessmentListRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var riskAssessmentList = _riskAssessmentListRepository.ListAsync();

            return Ok(riskAssessmentList);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _riskAssessmentListRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"'{ex}'\nAn error occurred while deleting the user." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(RiskAssessmentList riskAssessmentList)
        {
            if (ModelState.IsValid)
            {
                await _riskAssessmentListRepository.CreateAsync(riskAssessmentList);
                return NoContent();
            }


            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] RiskAssessmentList riskAssessmentList)
        {
            if (ModelState.IsValid)
            {
                var assessment = await _riskAssessmentListRepository.FindByAsync(id);
                if (assessment == null)
                {
                    return NotFound();
                }

                assessment.Name = riskAssessmentList.Name;
                assessment.Description = riskAssessmentList.Description;
                assessment.RiskIdentificationId = riskAssessmentList.RiskIdentificationId;
                assessment.ControlId = riskAssessmentList.ControlId;
                assessment.ResidualRiskRating = riskAssessmentList.ResidualRiskRating;

                await _riskAssessmentListRepository.UpdateAsync(assessment);
                return NoContent();
            }

            return BadRequest();
        }
    }
}
