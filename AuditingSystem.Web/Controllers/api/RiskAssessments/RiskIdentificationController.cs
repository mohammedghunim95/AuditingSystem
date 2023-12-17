using AuditingSystem.Entities.AuditProcess;
using AuditingSystem.Entities.RiskAssessments;
using AuditingSystem.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuditingSystem.Web.Controllers.api.RiskAssessment
{
    [Route("api/[controller]")]
    [ApiController]
    public class RiskIdentificationController : ControllerBase
    {
        private readonly IBaseRepository<RiskIdentification> _riskIdentificationRepository;
        public RiskIdentificationController(IBaseRepository<RiskIdentification> riskIdentificationRepositoryRepository)
        {
            this._riskIdentificationRepository = riskIdentificationRepositoryRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var riskIdentification = _riskIdentificationRepository.ListAsync();

            return Ok(riskIdentification);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _riskIdentificationRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"'{ex}'\nAn error occurred while deleting the user." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(RiskIdentification riskIdentification)
        {
            if (ModelState.IsValid)
            {
                await _riskIdentificationRepository.CreateAsync(riskIdentification);
                return NoContent();
            }


            return BadRequest();
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] RiskIdentification riskIdentification)
        {
            if (ModelState.IsValid)
            {
                var identification = await _riskIdentificationRepository.FindByAsync(id);
                if (identification == null)
                {
                    return NotFound();
                }

                identification.Name = riskIdentification.Name;
                identification.Description = riskIdentification.Description;
                identification.RiskCategoryId = riskIdentification.RiskCategoryId;
                identification.RiskImpactId = riskIdentification.RiskImpactId;
                identification.RiskLikelihoodId = riskIdentification.RiskLikelihoodId;
                identification.RiskRatingRationalization = riskIdentification.RiskRatingRationalization;
                identification.InherentRiskRating = riskIdentification.InherentRiskRating;

                await _riskIdentificationRepository.UpdateAsync(identification);
                return NoContent();
            }

            return BadRequest();
        }
    }
}
