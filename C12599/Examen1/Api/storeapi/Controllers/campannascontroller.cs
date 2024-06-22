using Microsoft.AspNetCore.Mvc;
using storeapi.Database;
using storeapi.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using storeapi.Hubs;

namespace storeapi
{
    [ApiController]
    [Route("api/[controller]")]
    public class CampannasController : ControllerBase
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public CampannasController(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> PostCampanna([FromBody] Campanna campanna)
        {
            if (campanna == null)
            {
                return BadRequest("Campanna object is null");
            }

            CampannaDB.InsertCampanna(campanna);
            await _hubContext.Clients.All.SendAsync("UpdateCampaigns", campanna.ContenidoHtml);

            return Ok(campanna); // Returns the received object as confirmation
        }

        [HttpGet]
        public IActionResult GetCampannas()
        {
            IEnumerable<Campanna> campannas = Campanna.LoadCampannasFromDatabase();
            return Ok(campannas);
        }
    }
}
