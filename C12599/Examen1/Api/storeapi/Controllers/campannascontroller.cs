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
        private readonly IHubContext<StoreHub> _hubContext;

        public CampannasController(IHubContext<StoreHub> hubContext)
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
            await _hubContext.Clients.All.SendAsync("UpdateCampaigns", campanna.ContenidoHtml, campanna.Estado);

            return Ok(campanna);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateCampannaEstado(int id, [FromBody] Campanna campanna)
        {
            if (campanna == null || id != campanna.Id)
            {
                return BadRequest("Invalid campanna object or ID");
            }

            CampannaDB.UpdateCampannaEstado(id, campanna.Estado);
            Campanna updatedCampanna = CampannaDB.GetCampannaById(id);
            await _hubContext.Clients.All.SendAsync("UpdateCampaigns", updatedCampanna.ContenidoHtml, updatedCampanna.Estado);

            return Ok(campanna);
        }

        [HttpGet]
        public IActionResult GetCampannas()
        {
            IEnumerable<Campanna> campannas = Campanna.LoadCampannasFromDatabase();
            return Ok(campannas);
        }
    }
}
