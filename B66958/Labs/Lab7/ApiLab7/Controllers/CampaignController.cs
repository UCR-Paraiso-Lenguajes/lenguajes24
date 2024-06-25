using Microsoft.AspNetCore.Mvc;

namespace ApiLab7.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CampaignsController : ControllerBase
    {
        private readonly WebSocketService _webSocketService;
        private CampaignBusiness campaignBusiness;

        public CampaignsController(WebSocketService webSocketService)
        {
            _webSocketService = webSocketService;
            campaignBusiness = new CampaignBusiness();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCampaignAsync([FromBody] Campaign campaign)
        {
            if (campaign == null)
                throw new ArgumentException("Please provide a campaign");
            var campaignResult = await campaignBusiness.AddCampaignAsync(campaign.Text);
            _webSocketService.SendMessage(campaignResult);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampaignAsync(String id)
        {
            if (id == null)
                throw new ArgumentException("Please provide a campaign id");
            var deleted = await campaignBusiness.RemoveCampaignAsync(id);
            _webSocketService.SendMessage(deleted);
            return Ok(deleted);
        }

        [HttpGet]
        public async Task<IEnumerable<Campaign>> GetCampaignsAsync()
        {
            IEnumerable<Campaign> campaigns = await campaignBusiness.GetCampaignsAsync();
            return campaigns;
        }
    }
}
