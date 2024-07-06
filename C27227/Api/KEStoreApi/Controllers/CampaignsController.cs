using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KEStoreApi;
using KEStoreApi.Data;

namespace KEStoreApi.Controllers
{
    [ApiController]
    [Route("api/")]
    public class CampaignsController : ControllerBase
    {
        private readonly CampaignService _campaignService;

        public CampaignsController(CampaignService campaignService)
        {
            _campaignService = campaignService;
        }

        [HttpGet("campaign")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CampaignMessage>>> GetAllCampaigns()
        {
            var campaigns = await _campaignService.GetAllCampaignsAsync();
            return Ok(campaigns);
        }

        [HttpGet("campaign/top3")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CampaignMessage>>> GetTop3Campaigns()
        {
            var campaigns = await _campaignService.GetLatestTop3CampaignsAsync();
            return Ok(campaigns);
        }

        [HttpPost("campaign")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CampaignMessage>> CreateCampaign([FromBody] CampaignMessage campaignMessage)
        {
            try
            {
                var createdCampaign = await _campaignService.AddCampaignAsync(campaignMessage);
                return CreatedAtAction(nameof(GetAllCampaigns), new { id = createdCampaign.Id }, createdCampaign);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("campaign/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCampaign(int id)
        {
            try
            {
                var isDeleted = await _campaignService.RemoveCampaignAsync(id);
                if (isDeleted)
                {
                    return NoContent();
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
