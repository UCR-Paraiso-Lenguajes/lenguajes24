using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using StoreAPI.Hubs;
using StoreAPI.Database;
using StoreAPI.models;

namespace StoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CampaignController : ControllerBase
    {
        private readonly IHubContext<CampaignHub> _campaignHubContext;
        private readonly CampaignDB _campaignDB;

        public CampaignController(IHubContext<CampaignHub> campaignHubContext, CampaignDB campaignDB)
        {
            _campaignHubContext = campaignHubContext;
            _campaignDB = campaignDB;
        }

        [HttpPost]
        public async Task<IActionResult> SendUpdate([FromBody] Campaign update)
        {
            if (update == null) return BadRequest(new { Message = "Update cannot be empty" });


            await _campaignDB.SaveAsync(update);
            await _campaignHubContext.Clients.All.SendAsync("ReceiveCampaignUpdate", update.Update);
            return Ok(new { Message = "Update sent" });
        }

        [HttpGet]
        public async Task<IActionResult> GetLatestMessages()
        {
            var messages = await _campaignDB.GetLatestMessagesAsync();
            return Ok(messages);
        }

        // POST: api/Campaign/Delete/5
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            try
            {
                await _campaignDB.LogicalDeleteAsync(id);
                return Ok("Message deleted successfully (logically).");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting message: {ex.Message}");
            }
        }
    }
}
